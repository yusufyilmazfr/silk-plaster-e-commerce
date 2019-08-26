using Ninject.Modules;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Manager;
using SilkPlaster.Common.Services.Hash;
using SilkPlaster.Common.Services.Hash.Md5;
using SilkPlaster.Common.Services.Mail;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Abstract.UnitOfWork;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Context;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete.DependencyResolver.Ninject
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<DbContext>().To<DatabaseContext>().InThreadScope();

            Bind<IHashGeneratorService>().To<Md5GeneratorService>();

            Bind<IMailService>().To<MailService>();

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
            Bind<IInComingMailManager>().To<InComingMailManager>();


            Bind<IAddressDal>().To<EfAddressDal>();
            Bind<IAdminDal>().To<EfAdminDal>();
            Bind<IBasketDal>().To<EfBasketDal>();
            Bind<ICategoryDal>().To<EfCategoryDal>();
            Bind<ICityDal>().To<EfCityDal>();
            Bind<ICommentDal>().To<EfCommentDal>();
            Bind<ICountyDal>().To<EfCountyDal>();
            Bind<IMemberDal>().To<EfMemberDal>();
            Bind<IOrderDal>().To<EfOrderDal>();
            Bind<IOrderDetailDal>().To<EfOrderDetailDal>();
            Bind<IProductDal>().To<EfProductDal>();
            Bind<ISliderDal>().To<EfSliderDal>();
            Bind<IWishListDal>().To<EfWishListDal>();
            Bind<IInComingMailDal>().To<EfInComingMailDal>();
        }
    }
}
