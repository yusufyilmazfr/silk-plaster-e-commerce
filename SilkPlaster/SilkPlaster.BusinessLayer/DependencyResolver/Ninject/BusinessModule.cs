using Ninject.Modules;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.DependencyResolver.Ninject
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAddressManager>().To<AddressManager>();
            Bind<IAdminManager>().To<AdminManager>();
            Bind<IBasketManager>().To<BasketManager>();
            Bind<ICategoryManager>().To<CategoryManager>();
            Bind<ICityManager>().To<CityManager>();
            Bind<ICommentManager>().To<CommentManager>();
            Bind<ICountyManager>().To<CountyManager>();
            Bind<IMemberManager>().To<MemberManager>();
            Bind<IOrderManager>().To<OrderManager>();
            Bind<IOrderDetailManager>().To<OrderDetailManager>();
            Bind<IProductManager>().To<ProductManager>();
            Bind<ISliderManager>().To<SliderManager>();
            Bind<IWishListManager>().To<WishListManager>();


            Bind<IAddressDal>().To<AddressDal>();
            Bind<IAdminDal>().To<AdminDal>();
            Bind<IBasketDal>().To<BasketDal>();
            Bind<ICategoryDal>().To<CategoryDal>();
            Bind<ICityDal>().To<CityDal>();
            Bind<ICommentDal>().To<CommentDal>();
            Bind<ICountyDal>().To<CountyDal>();
            Bind<IMemberDal>().To<MemberDal>();
            Bind<IOrderDal>().To<OrderDal>();
            Bind<IOrderDetailDal>().To<OrderDetailDal>();
            Bind<IProductDal>().To<ProductDal>();
            Bind<ISliderDal>().To<SliderDal>();
            Bind<IWishListDal>().To<WishListDal>();
        }
    }
}
