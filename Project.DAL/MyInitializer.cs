using Project.DAL.Context;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Project.DAL
//{
//    public class MyInitializer : CreateDatabaseIfNotExists<MyContext>
//    {

        //protected override void Seed(MyContext context)
        //{


        //    for (int i = 0; i < 10; i++)
        //    {
        //        AppUser user = new AppUser()
        //        {
        //            ActivationCode = Guid.NewGuid(),
        //            Active = false,
        //            Email = "ibrahimoguzhany@gmail.com",
        //            UserName = FakeData.NameData.GetFirstName(),
        //            Password = "123",
        //            CreatedDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now.AddHours(1),

        //        };
        //        for (int w = 0; w < 10; w++)
        //        {
        //            Shipper ship = new Shipper()
        //            {
        //                CompanyName = FakeData.TextData.GetSentence(),
        //                Phone = FakeData.PhoneNumberData.GetPhoneNumber(),


        //            };
        //            for (int k = 0; k < 10; k++)
        //            {
        //                UserProfile profile = new UserProfile()
        //                {
        //                    Address = FakeData.PlaceData.GetAddress(),
        //                    FirstName = FakeData.NameData.GetFirstName(),
        //                    LastName = FakeData.NameData.GetSurname(),
        //                    AppUser = user,
                            
                            
        //                };
        //            }

        //            for (int q = 0; q < 10; q++)
        //            {
        //                OrderDetail orderDetail = new OrderDetail()
        //                {
        //                    Quantity = Convert.ToInt16(FakeData.NumberData.GetNumber()),
                                                   
        //                    Shipper = ship,
        //                    TotalPrice = 2000,
                                          

        //                };
        //            }


        //        }
        //        for (int c = 0; c < 10; c++)
        //        {
        //            Order order = new Order()
        //            {
        //                AppUser = user,
        //                Email = "ibrahimoguzhany@gmail.com",
        //                ShippedAddress = FakeData.PlaceData.GetAddress(),  

        //            };
        //            for (int j = 0; j < 10; j++)
        //            {
        //                OrderDetail Cargo = new OrderDetail()
        //                {
        //                    Order = order,
                            
                            

        //                };

        //            }
        //        }

        //        for (int b = 0; b < 10; b++)
        //        {//Adding Categories
        //            Category cat = new Category()
        //            {
        //                CategoryName = FakeData.PlaceData.GetCounty(),
        //                Description = FakeData.TextData.GetSentence(),

        //            };
        //            // Adding Products 
        //            for (int a = 0; a < 10; a++)
        //            {
        //                Product product = new Product()
        //                {
        //                    Category = cat,
        //                    ImagePath = FakeData.NetworkData.GetDomain(),
        //                    ProductName = FakeData.NameData.GetCompanyName(),
        //                    UnitPrice = FakeData.NumberData.GetNumber(),
        //                    UnitsInStock = 10,
                            


        //                };
        //            }
        //        }


                

        
    //    }
    //}

