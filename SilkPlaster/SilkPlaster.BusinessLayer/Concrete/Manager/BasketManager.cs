using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Abstract.UnitOfWork;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete.Manager
{
    public class BasketManager : IBasketManager
    {
        private IBasketDal _basketDal { get; set; }
        private IProductManager _productManager { get; set; }
        private IMemberManager _memberManager { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }

        private BusinessLayerResult<Basket> _layerResult { get; set; }

        public BasketManager(IBasketDal basketDal, IProductManager productManager, IMemberManager memberManager, IUnitOfWork unitOfWork)
        {
            _basketDal = basketDal;
            _productManager = productManager;
            _memberManager = memberManager;
            _unitOfWork = unitOfWork;

            _layerResult = new BusinessLayerResult<Basket>();
        }

        public BusinessLayerResult<Basket> DeleteBasketItem(Basket obj)
        {
            _layerResult.Result = GetBasketItemWithByProductId(obj.ProductId, obj.MemberId);

            if (ObjectHelper.ObjectIsNull(_layerResult.Result))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir ürün bulunmamaktadır!");
                return _layerResult;
            }

            //_basketDal.Delete(_layerResult.Result);

            int count = _unitOfWork.SaveChanges();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToDeleteRecord, "Ürün silinemedi!");
            }
            return _layerResult;
        }

        public BusinessLayerResult<Basket> AddProductInBasket(int memberId, int productId, int productCount)
        {
            if (productId <= 0 || productCount <= 0 || memberId <= 0)
            {
                _layerResult.AddError(ErrorMessageCode.ValuesNotCorrect, "Geçerli değerler giriniz!");
                return _layerResult;
            }

            Product product = _productManager.GetProductById(productId);
            Member member = _memberManager.GetMemberById(memberId);

            if (member == null)
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kullanıcı bulunmamaktadır!");
            if (product == null)
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir ürün bulunmamaktadır!");
            else if (!product.IsContinued)
                _layerResult.AddError(ErrorMessageCode.ClosedForSale, $"{product.Name} satışa kapanmıştır!");
            else if (!product.InStock)
                _layerResult.AddError(ErrorMessageCode.OutOfStock, $"{product.Name} stokta bulunmamatadır!");
            else if (product.Quantity < productCount)
                _layerResult.AddError(ErrorMessageCode.QuantityOverflow, $"{product.Name} üründen en fazla {product.Quantity} adet alabilirsiniz!");

            if (_layerResult.HasError())
                return _layerResult;

            //The process of finding product in the basket.
            Basket basketItem = GetBasketItemWithByProductId(productId, memberId);

            int count = 0;

            if (!ObjectHelper.ObjectIsNull(basketItem))
            {
                int totalProductCountInBasket = basketItem.ProductCount + productCount;

                if (totalProductCountInBasket > product.Quantity)
                {
                    _layerResult.AddError(ErrorMessageCode.QuantityOverflow, $"{product.Name} üründen en fazla {product.Quantity} adet alabilirsiniz!");
                    return _layerResult;
                }

                basketItem.ProductCount += productCount;

                _basketDal.Update(basketItem);

                count = _unitOfWork.SaveChanges();

                //count = _basketDal.Save();

                if (count == 0)
                {
                    _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "İşlem gerçekleştirilemedi!");
                }
            }

            else
            {
                Basket basket = new Basket()
                {
                    ProductId = productId,
                    ProductCount = productCount,
                    MemberId = memberId
                };

                _basketDal.Insert(basket);

                count = _unitOfWork.SaveChanges();
                //count = _basketDal.Save();

                if (count == 0)
                {
                    _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Ekleme işlemi gerçekleştirilemedi!");
                }
            }
            return _layerResult;

        }

        public Basket GetBasketItemWithByMemberId(int basketItemId, int memberId)
        {
            return _basketDal.Find(i => i.Id == basketItemId && i.MemberId == memberId);
        }

        public Basket GetBasketItemWithByProductId(int productId, int memberId)
        {
            return _basketDal.Find(i => i.ProductId == productId && i.MemberId == memberId);
        }

        public List<Basket> GetBasketItemsByMemberId(int memberId)
        {
            return _basketDal.GetBasketItemsByMemberId(memberId);
        }

        public int GetBasketItemsCountByMemberId(int memberId)
        {
            return GetBasketItemsByMemberId(memberId).Count;
        }

        public BusinessLayerResult<Basket> DecreaseProductCount(int memberId, int productId, int increaseCount = 1)
        {
            if (productId <= 0 || memberId <= 0 || increaseCount < 1)
            {
                _layerResult.AddError(ErrorMessageCode.ValuesNotCorrect, "Geçerli değerler giriniz!");
                return _layerResult;
            }

            Product product = _productManager.GetProductById(productId);
            Member member = _memberManager.GetMemberById(memberId);

            if (member == null)
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kullanıcı bulunmamaktadır!");
            if (product == null)
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir ürün bulunmamaktadır!");
            else if (!product.IsContinued)
                _layerResult.AddError(ErrorMessageCode.ClosedForSale, $"{product.Name} satışa kapanmıştır!");
            else if (!product.InStock)
                _layerResult.AddError(ErrorMessageCode.OutOfStock, $"{product.Name} stokta bulunmamatadır!");

            if (_layerResult.HasError())
                return _layerResult;

            //The process of finding product in the basket.
            Basket basketItem = GetBasketItemWithByProductId(productId, memberId);

            int count = 0;

            if (!ObjectHelper.ObjectIsNull(basketItem))
            {
                int totalProductCountInBasket = basketItem.ProductCount - increaseCount;

                if (totalProductCountInBasket <= 0)
                {
                    DeleteBasketItem(basketItem);
                    return _layerResult;
                }

                basketItem.ProductCount -= increaseCount;

                _basketDal.Update(basketItem);

                count = _unitOfWork.SaveChanges();
                //count = _basketDal.Save();

                if (count == 0)
                {
                    _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "İşlem gerçekleştirilemedi!");
                }
            }
            else
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kayıt bulunmamaktadır!");
            }
            return _layerResult;
        }

        public void DeleteBasketItemById(int Id)
        {
            Basket basket = GetBasketItemById(Id);
            _basketDal.Delete(basket);
        }

        public Basket GetBasketItemById(int Id)
        {
            return _basketDal.Find(i => i.Id == Id);
        }
    }
}
