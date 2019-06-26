using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.EntityFramework
{
    public class MyInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Random random = new Random();
            DateTime dateTime = DateTime.Now;

            Member member = new Member()
            {
                FirstName = "Yusuf",
                LastName = "Yılmaz",
                Email = "yusufyilmazfr@gmail.com",
                Password = "123",
                AddedDate = dateTime,
                ModifiedDate = dateTime
            };

            context.Members.Add(member);
            context.SaveChanges();


            // Fake member data inserting

            for (int i = 0; i <= 10; i++)
            {
                dateTime = FakeData.DateTimeData.GetDatetime();

                Member tempMember = new Member()
                {
                    FirstName = FakeData.NameData.GetFirstName(),
                    LastName = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    Password = FakeData.NumberData.GetNumber(2656).ToString(),
                    AddedDate = dateTime,
                    ModifiedDate = dateTime
                };

                context.Members.Add(tempMember);
            }


            // Fake category data inserting

            for (int i = 0; i <= 10; i++)
            {
                Category category = new Category()
                {
                    Name = FakeData.NameData.GetCompanyName(),
                    Description = FakeData.TextData.GetAlphabetical(50),
                    AddedDate = FakeData.DateTimeData.GetDatetime(),
                    ModifiedDate = DateTime.Now
                };

                context.Categories.Add(category);
            }

            context.SaveChanges();

            // Fake product data inserting

            for (int i = 0; i <= 60; i++)
            {
                Product product = new Product()
                {
                    Name = FakeData.TextData.GetAlphabetical(200),
                    ShortDescription = FakeData.TextData.GetAlphabetical(200),
                    LongDescription = FakeData.TextData.GetAlphabetical(200),
                    LastPrice = FakeData.NumberData.GetNumber(0, 250),
                    NewPrice = FakeData.NumberData.GetNumber(0, 250),
                    InStock = true,
                    FirstImage = random.Next(1, 20) + ".jpg",
                    IsContinued = true,
                    IsFeatured = i % 5 == 0 ? true : false,
                    AddedDate = FakeData.DateTimeData.GetDatetime(),
                    ModifiedDate = DateTime.Now,
                    CategoryId = random.Next(1, 10),
                    Quantity = random.Next(40, 80),
                    ProductImages = new List<ProductImage>()
                    {
                        new ProductImage{Name = "5.jpg"},
                        new ProductImage{Name = "7.jpg"},
                        new ProductImage{Name = "11.jpg"},
                        new ProductImage{Name = "13.jpg"},
                        new ProductImage{Name = "15.jpg"},
                        new ProductImage{Name = "17.jpg"},
                        new ProductImage{Name = "9.jpg"},
                        new ProductImage{Name = "2.jpg"},
                        new ProductImage{Name = "4.jpg"},
                        new ProductImage{Name = "10.jpg"}
                    }
                };

                context.Products.Add(product);
            }

            context.SaveChanges();

            // Fake comment data inserting

            for (int i = 0; i < 200; i++)
            {
                Comment comment = new Comment()
                {
                    MemberId = random.Next(1, 10),
                    Text = FakeData.TextData.GetAlphabetical(250),
                    ProductId = random.Next(1, 60),
                    AddedDate = FakeData.DateTimeData.GetDatetime(),
                    ModifiedDate = DateTime.Now,
                    StarCount = (byte)random.Next(0, 5),
                };

                context.Comments.Add(comment);
            }

            context.SaveChanges();


            for (int i = 0; i < 80; i++)
            {

                int num = random.Next(1, 60);
                Product product = context.Products.FirstOrDefault(k => k.Id == num);

                Order tempOrder = new Order
                {
                    OrderNumber = random.Next(0, 999999).ToString(),
                    Description = FakeData.TextData.GetAlphabetical(50),
                    MemberId = random.Next(1, 10),
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            ProductId = product.Id,
                            Price = product.NewPrice,
                            Quantity = random.Next(1,10)
                        }
                    },

                };

                context.Orders.Add(tempOrder);
            }

            context.SaveChanges();



            for (int i = 0; i < 10; i++)
            {
                WishList wishList = new WishList()
                {
                    MemberId = random.Next(1, 10),
                    ProductId = random.Next(1, 60)
                };

                context.WishLists.Add(wishList);
            }

            Admin admin = new Admin
            {
                FirstName = "Yusuf",
                LastName = "Yılmaz",
                Email = "yusufyilmazfr@gmail.com",
                Password = "123"
            };

            Admin admin2 = new Admin
            {
                FirstName = "Halit",
                LastName = "Akkuş",
                Email = "halit@halit.org",
                Password = "123"
            };

            context.Admins.Add(admin);
            context.Admins.Add(admin2);

            context.SaveChanges();
        }
    }
}
