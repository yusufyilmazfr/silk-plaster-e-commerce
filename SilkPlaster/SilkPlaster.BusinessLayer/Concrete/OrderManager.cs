using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.Common.OrderMessageObj;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete
{
    public class OrderManager : IOrderManager
    {
        private IOrderDal _orderDal { get; set; }
        private IBasketDal _basketDal { get; set; }
        private IProductManager _productManager { get; set; }

        public BusinessLayerResult<Order> _layerResult { get; set; }

        public OrderManager(IOrderDal orderDal, IBasketManager basketManager, IBasketDal basketDal, IProductManager productManager)
        {
            _orderDal = orderDal;
            _basketDal = basketDal;
            _productManager = productManager;
            _layerResult = new BusinessLayerResult<Order>();
        }

        public void CreateNewOrder(OrderViewModel orderViewModel)
        {
            Order order = new Order()
            {
                MemberId = orderViewModel.MemberId,
                AddressId = orderViewModel.AddressId,
                Description = orderViewModel.Description,
                OrderNumber = orderViewModel.OrderNumber,
                CargoTrackingNumber = "-",
                OrderState = EnumOrderState.Waiting,
                PaymentType = EnumHelper.ConvertValueToEnumObject<EnumPaymentTypes, int>(orderViewModel.PaymentType),
                PaymentId = orderViewModel.PaymentId,
                PaymentToken = orderViewModel.PaymentToken,
                ConversationId = orderViewModel.ConservationId,
            };

            order.OrderDetails = new List<OrderDetail>();

            foreach (var item in orderViewModel.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }

            _orderDal.Insert(order);
        }

        public BusinessLayerResult<Order> CheckOrder(OrderViewModel orderViewModel)
        {
            CreateNewOrder(orderViewModel);

            List<int> list = orderViewModel.OrderDetails.Select(i => i.Id).ToList();

            _basketDal.DeleteRange(list);

            int count = _orderDal.Save();

            if (count > 0)
            {
                count = _basketDal.Save();
                if (count == 0)
                    _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "İşlem yapılamadı!");
            }
            else
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "İşlem yapılamadı!");

            return _layerResult;
        }

        public List<Order> GetAllOrdersWithDetails()
        {
            return _orderDal.GetAllOrdersWithDetails();
        }

        public Order GetOrderById(int Id)
        {
            return _orderDal.Find(i => i.Id == Id);
        }

        public Order GetOrderDetailById(int Id)
        {
            return _orderDal.GetOrderDetailById(Id);
        }

        public BusinessLayerResult<Order> ChangeTrackingNumberAndState(int orderId, string orderTrackingNumber, EnumOrderState newOrderState)
        {
            _layerResult.Result = GetOrderDetailById(orderId);

            EnumOrderState lastOrdeState = _layerResult.Result.OrderState;

            if (ObjectHelper.ObjectIsNull(newOrderState))
                _layerResult.AddError(ErrorMessageCode.ValuesNotCorrect, "Böyle bir sipariş tipi bulunmamaktadır!");

            if (ObjectHelper.ObjectIsNull(_layerResult.Result))
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir sipariş bulunmamaktadır!");

            if (_layerResult.HasError())
                return _layerResult;


            if (IncreaseTheProductCount(lastOrdeState, newOrderState))
            {
                var orderDetails = _layerResult.Result.OrderDetails;

                foreach (var item in orderDetails)
                {
                    Product product = _productManager.GetProductById(item.ProductId);
                    product.Quantity += item.Quantity;
                    var tempResult = _productManager.Update(product);

                    tempResult.Errors.ForEach(x => _layerResult.AddError(x.ErrorCode, x.ErrorMessage));
                }
            }

            else if (DecreaseTheProductCount(lastOrdeState, newOrderState))
            {
                var orderDetails = _layerResult.Result.OrderDetails;

                foreach (var item in orderDetails)
                {
                    Product product = _productManager.GetProductById(item.ProductId);
                    product.Quantity -= item.Quantity;
                    var tempResult = _productManager.Update(product);

                    tempResult.Errors.ForEach(x => _layerResult.AddError(x.ErrorCode, x.ErrorMessage));
                }
            }

            if (_layerResult.HasError())
                return _layerResult;


            _layerResult.Result.CargoTrackingNumber = orderTrackingNumber;
            _layerResult.Result.OrderState = newOrderState;

            _orderDal.Update(_layerResult.Result);
            int count = _orderDal.Save();

            if (count == 0)
                _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Sipariş güncellenemedi!");

            return _layerResult;
        }

        private bool IncreaseTheProductCount(EnumOrderState lastOrdeState, EnumOrderState newOrderState)
        {
            //will be refactoring here for variable name:))

            bool x = (EnumOrderState.Canceled == newOrderState || EnumOrderState.Dispatch == newOrderState);
            bool y = (EnumOrderState.Preparing == lastOrdeState || EnumOrderState.SendToCargo == lastOrdeState || EnumOrderState.Completed == lastOrdeState);

            return x && y;
        }

        private bool DecreaseTheProductCount(EnumOrderState lastOrdeState, EnumOrderState newOrderState)
        {
            //will be refactoring here for variable name :))

            bool x = (EnumOrderState.Waiting == lastOrdeState || EnumOrderState.Dispatch == lastOrdeState || EnumOrderState.Canceled == lastOrdeState);
            bool y = (EnumOrderState.Preparing == newOrderState || EnumOrderState.SendToCargo == newOrderState || EnumOrderState.Completed == newOrderState);

            return x && y;
        }

        public List<Order> GetOrdersByMemberId(int Id)
        {
            return _orderDal.GetAll(i => i.MemberId == Id);
        }

        public Order GetOrderDetailByMemberId(int orderId, int memberId)
        {
            return _orderDal.GetOrderDetailMemberId(orderId, memberId);
        }
    }
}
