using Project.BLL.DesignPatterns;
using Project.DAL.Context;
using Project.DTOTools.DTOModels;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace Project.APIPL.Controllers
{
    public class CargoController : ApiController
    {
        private MyContext _db;
        public CargoController()
        {
            _db = DBTool.DBInstance;
        }
        public IQueryable<CargoDTO> GetCargos()
        {
            //return _db.OrderDetail.ToList().Select(Mapper.Map<OrderDetail, OrderDetailDTO>).AsQueryable();
            var cargos = from s in _db.OrderDetail
                         select new CargoDTO
                         {
                             BuyerFirstName = s.Order.AppUser.Profile.FirstName,
                             BuyerLastName = s.Order.AppUser.Profile.LastName,
                             TotalPrice = s.TotalPrice,
                             ProductName = s.Product.ProductName,
                             Address = s.Order.ShippedAddress,
                             Email = s.Order.Email,
                             Quantity = s.Quantity,
                             ShipperCompanyName = s.Order.Shipper.CompanyName,
                             ShipperPhoneNumber = s.Order.Shipper.Phone,

                         };
            return cargos.AsQueryable();
        }



        public IHttpActionResult GetCargo(int id)
        {
            var cargo = _db.OrderDetail.Select(s =>
               new CargoDTO()
               {
                   ID = s.ID,
                   BuyerFirstName = s.Order.AppUser.Profile.FirstName,
                   BuyerLastName = s.Order.AppUser.Profile.LastName,
                   TotalPrice = s.TotalPrice,
                   ProductName = s.Product.ProductName,
                   Address = s.Order.ShippedAddress,
                   Email = s.Order.Email,
                   Quantity = s.Quantity,
                   ShipperCompanyName = s.Order.Shipper.CompanyName,
                   ShipperPhoneNumber = s.Order.Shipper.Phone,
               }).SingleOrDefault(b => b.ID == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        [HttpPost]
        public IHttpActionResult CreateCargo(OrderDetail orderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.OrderDetail.Add(orderDetail);
            _db.SaveChanges();


            var dto = new CargoDTO()
            {
                Address = orderDetail.Order.ShippedAddress,
                BuyerFirstName = orderDetail.Order.AppUser.Profile.FirstName,
                BuyerLastName = orderDetail.Order.AppUser.Profile.LastName,
                Email = orderDetail.Order.Email,
                ProductName = orderDetail.Product.ProductName,
                Quantity = orderDetail.Quantity,
                ShipperCompanyName = orderDetail.Order.Shipper.CompanyName,
                ShipperPhoneNumber = orderDetail.Order.Shipper.Phone,
                TotalPrice = orderDetail.TotalPrice,

            };

            return CreatedAtRoute("DefaultApi", new { id = orderDetail.ID }, dto);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCargo(int id)
        {
            var orderDetailInIDB = _db.OrderDetail.FirstOrDefault(x => x.ID == id);

            if (orderDetailInIDB != null)
            {
                _db.OrderDetail.Remove(orderDetailInIDB);
                _db.SaveChanges();

            }
            return Ok("SILINDI");
        }

        [HttpPut]
        public void UpdateCargo(int id, CargoDTO cargoDTO)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var orderDetailInDB = _db.OrderDetail.FirstOrDefault(x => x.ID == id);

            if (orderDetailInDB != null)
            {
                var cargo = from s in _db.OrderDetail
                            select new CargoDTO
                            {
                                TotalPrice = s.TotalPrice,
                                Quantity = s.Quantity,
                                ProductName = s.Product.ProductName,
                                ShipperCompanyName = s.Order.Shipper.CompanyName,
                                Address = s.Order.ShippedAddress,
                                BuyerFirstName = s.Order.AppUser.Profile.FirstName,
                                BuyerLastName = s.Order.AppUser.Profile.LastName,
                                Email = s.Order.Email,
                            };
                _db.SaveChanges();
            }
        }
    }
}






/* Mapper.Map<OrderDetailDTO, OrderDetail>(cargoDTO, orderDetailInDB); *///varolan bir obje varsa 2.bir arguman olarak buraya verebiliriz.

//yukardaki satir sayesinde bu islem otomatik saglandi ve gerek kalmadi.


//        _db.SaveChanges();


//    }



//}



