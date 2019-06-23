using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.EntityFramework;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer
{
    public class BasketManager : ManagerBase<Basket>
    {
        BusinessLayerResult<Basket> _layerResult = new BusinessLayerResult<Basket>();

        public new BusinessLayerResult<Basket> Delete(Basket obj)
        {
            _layerResult.Result = base.Find(i => i.MemberId == obj.MemberId && i.ProductId == obj.ProductId);

            if (_layerResult.Result == null)
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir ürün bulunmamaktadır!");
                return _layerResult;
            }

            int count = base.Delete(_layerResult.Result);

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

            Repository<Product> productRepository = new Repository<Product>();
            Repository<Member> memberRepository = new Repository<Member>();

            Product product = productRepository.Find(i => i.Id == productId);
            Member member = memberRepository.Find(i => i.Id == memberId);

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

            if (_layerResult.Errors.Count > 0)
                return _layerResult;

            //The process of finding product in the basket.
            Basket basketItem = base.Find(i => i.ProductId == productId && i.MemberId == memberId);

            int count = 0;

            if (basketItem != null)
            {
                int totalProductCountInBasket = basketItem.ProductCount + productCount;

                if (totalProductCountInBasket > product.Quantity)
                {
                    _layerResult.AddError(ErrorMessageCode.QuantityOverflow, $"{product.Name} üründen en fazla {product.Quantity} adet alabilirsiniz!");
                    return _layerResult;
                }

                basketItem.ProductCount += productCount;

                count = base.Update(basketItem);

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

                count = base.Insert(basket);

                if (count == 0)
                {
                    _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Ekleme işlemi gerçekleştirilemedi!");
                }
            }
            return _layerResult;

        }
    }
}
