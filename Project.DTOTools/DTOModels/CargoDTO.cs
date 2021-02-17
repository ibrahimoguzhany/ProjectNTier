using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DTOTools.DTOModels
{
    public class CargoDTO
    {
        public int ID { get; set; }
        public string BuyerFirstName { get; set; }
        public string BuyerLastName { get; set; }

        public string ProductName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public short Quantity { get; set; }
        public string ShipperCompanyName { get; set; }
        public string ShipperPhoneNumber { get; set; }



        //Relational Properties

        //public virtual Shipper Shipper { get; set; }
        //public virtual UserProfile Customer { get; set; }



    }
}
