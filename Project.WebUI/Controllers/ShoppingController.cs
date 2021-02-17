using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.WebUI.Models;
using Project.WebUI.Models.ShoppingTools;
using Project.ENTITIES.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Project.COMMON.Tools;

namespace Project.WebUI.Controllers
{
    public class ShoppingController : Controller
    {
        OrderRepository _oRep;
        ProductRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;

        public ShoppingController()
        {
            _oRep = new OrderRepository();
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
            _odRep = new OrderDetailRepository();
        }


        // GET: Shopping

        //Sayfalama işlemleri yapmak icin Pagination kütüphanesinden yararlanıyoruz(PagedList)
        public ActionResult ShoppingList(int? page, int? categoryID) //nullable int vermemizin sebebi buradaki int'in sayfa sayımızı temsil edecek olmasıdır...Ancak birsi direkt alısveris sayfasına ulastıgında sayfa sayısını göndermeyeceginden bu şekilde de Action'in calısabilmesini istiyoruz...
        {
            //string a = "Çağrı";
            //string b = a ?? "Mehmet"; //eger a degişkeninin degeri null ise b'ye Mehmet'i at...Eger null degilse b'ye git a'nın degerini demektir...
            PAVM pavm = new PAVM()
            {
                PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9),
                Categories = _cRep.GetActives()
            };
            if (categoryID != null) TempData["catID"] = categoryID;
            return View(pavm);
        }

        //https://localhost:44365/api/Payment/ReceivePayment/

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

            Product eklenecekUrun = _pRep.Find(id);

            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };

            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }

        public ActionResult CartPage()
        {
            if (Session["scart"] != null)
            {
                CartPageVM cpvm = new CartPageVM();
                Cart c = Session["scart"] as Cart;

                cpvm.Cart = c;
                return View(cpvm);
            }

            TempData["sepetBos"] = "Sepetinizde Urun Bulunmamaktadir";
            return RedirectToAction("ShoppingList");
        }

        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;

                c.SepettenSil(id);
                if (c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizde urun bulunmamaktadir";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }

            return RedirectToAction("ShoppingList");
        }

        public ActionResult SiparisOnayla()
        {
            AppUser mevcutKullanici;

            if (Session["member"] != null)
            {
                mevcutKullanici = Session["member"] as AppUser;
            }
            else
            {
                TempData["anonim"] = "Kullanici uye degil";
            }
            return View();
        }

        [HttpPost]
        public ActionResult SiparisiOnayla(OrderVM ovm)
        {
            bool result;

            Cart sepet = Session["scart"] as Cart;

            ovm.Order.TotalPrice = ovm.PaymentDTO.ShoppingPrice = sepet.TotalPrice.Value;

            //API kullanimi

            //WebApiRestService.WebAPIClient

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44365/api/");

                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentDTO);

                HttpResponseMessage sonuc;

                try
                {
                    sonuc = postTask.Result;
                }
                catch (Exception ex)
                {
                    TempData["baglantiRed"] = "Banka baglantiyi reddetti.";
                    return RedirectToAction("ShoppingList");
                }

                if (sonuc.IsSuccessStatusCode) result = true;

                else result = false;

                if(result)
                {
                    if(Session["member"]!= null)
                    {
                        AppUser kullanici = Session["member"] as AppUser;
                        ovm.Order.AppUserID = kullanici.ID;
                        ovm.Order.UserName = kullanici.UserName;
                    }
                    else
                    {
                        ovm.Order.AppUserID = null;
                        ovm.Order.UserName = TempData["anonim"].ToString();
                    }

                    _oRep.Add(ovm.Order); //OrderRepository bu noktada Order'i eklerken onun ID'sini olusturuyor.

                    foreach (CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _odRep.Add(od);

                        //Stoktan dusmesini istiyorsaniz

                        Product stokDus = _pRep.Find(item.ID);
                        stokDus.UnitsInStock -= item.Amount;
                        _pRep.Update(stokDus);

                    }

                    TempData["odeme"] = "siparisiniz bize ulasmistir. Tesekkur ederiz";
                    MailSender.Send(ovm.Order.Email, body: $"Siparisiniz basariyla alindi...{ovm.Order.TotalPrice}");
                    return RedirectToAction("ShoppingList");
                }
                else
                {
                    TempData["sorun"] = "Odeme ile ilgili bir sorun olustur.Lutfen bankanizla iletisime geciniz.";
                    return RedirectToAction("ShoppingList");
                }
            }
            return View();
        }
    }
}