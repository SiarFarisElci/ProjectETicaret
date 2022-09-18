using PagedList;
using Project.BLL.DesingPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Entities;
using Project.MVCUI.Models.ShoppingTools;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {

        OrderRepository _oRep;
        ProdcutRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;

        public ShoppingController()
        {
            _oRep = new OrderRepository();
            _odRep = new OrderDetailRepository();
            _pRep = new ProdcutRepository();
            _cRep = new CategoryRepository();
        }
        // GET: Shopping
        public ActionResult ShoppingList(int? page, int? categoryID)
        {
            PaginationVM pvm = new PaginationVM
            {
                PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9),
                Categories = _cRep.GetActives()
            };
            return View(pvm);
        }

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;
            Product eklenecekUrun = _pRep.Find(id);

            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProuctName,
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

            TempData["bos"] = "Sepetinizde Urun bulunmamaktadır";
            return   RedirectToAction("ShoppingList");
        }

        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] != null )
            {
                Cart c = Session["scart"] as Cart;
                c.SepetenCikar(id);
                if (c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizdeki tüm ürünler cıkarılmıstır";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }


            return RedirectToAction("ShoppingList");
        }

        public ActionResult ConfirmOrder()
        {
            AppUser currentUser;
            if (Session["member"] != null)
            {
                currentUser = Session["member"] as AppUser;
            }
            else TempData["anonim"] = "Kullanıcı üye degil";
            return View();


        }

        [HttpPost]
        public ActionResult ConfirmOrder(OrderVM ovm)
        {
            bool result;
            Cart sepet = Session["scart"] as Cart;
            ovm.Order.TotalPrice = ovm.PaymentDTO.ShoppingPrice = sepet.TotalPrice;


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/api/");
                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentDTO);
                HttpResponseMessage sonuc;
                try
                {
                    sonuc = postTask.Result;
                }
                catch (Exception ex)
                {
                    TempData["baglantiRed"] = "Banka Baglantiyi reddetti";
                    return RedirectToAction("ShoppingList");
                    
                }
                if (sonuc.IsSuccessStatusCode) result = true;
                else result = false;
                if (result)
                {
                    if (Session["member"] != null)
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

                    
                    _oRep.Add(ovm.Order);//OrderRepository bu noktada Order i eklerken onun ID sini olusturuyor.

                    foreach (CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _odRep.Add(od);
                        //Stoktanda dusurelim

                        Product stokDus = _pRep.Find(item.ID);
                        stokDus.UnitsInStock -= item.Amount;
                        _pRep.Updated(stokDus);
                    }

                    TempData["odeme"] = "Siparisiniz bize ulasmıstır...Tesekkur ederiz";
                    MailService.Send(ovm.Order.Email, body: $"Siparişiniz basarıyla alındı {ovm.Order.TotalPrice}");

                    Session.Remove("scart");
                    return RedirectToAction("ShoppingList");


                }
                else
                {
                    Task<string> s = sonuc.Content.ReadAsStringAsync();
                    TempData["sorun"] = s.Result;
                    return RedirectToAction("ShoppingList");
                }
            }

             
        }


    }


}


