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

        public BusinessLayerResult<Order> _layerResult { get; set; }

        public OrderManager(IOrderDal orderDal, IBasketManager basketManager, IBasketDal basketDal, IProductDal productDal)
        {
            _orderDal = orderDal;
            _basketDal = basketDal;
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
    }
}
