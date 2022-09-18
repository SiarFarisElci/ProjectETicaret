using Project.BLL.DesingPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Entities;
using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        AppUserRepository _apRep;

        public HomeController()
        {
            _apRep = new AppUserRepository();
        }

        // GET: Home
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AppUser appUser)
        {
            AppUser yakalanan = _apRep.FirstOrDefault(x=> x.UserName == appUser.UserName);
            if (yakalanan == null)
            {
                ViewBag.Kullanici = "Kullanıcı Bulunamadı";
                return View();
            }

            string decryted = DantexCrypt.DeCrypt(yakalanan.Password);
            if (appUser.Password == decryted && yakalanan.Role == UserRole.Admin)
            {
                if (!yakalanan.Active) return AktifKontrol();
                Session["member"] = yakalanan;
                return RedirectToAction("ShoppingList", "Shopping");
                
            }

            return View();
        }


        private ActionResult AktifKontrol()
        {
            ViewBag.Kullanici = "Lutfen Hesabınızı aktif hae getiriniz";
            return View("Login");
        }
    }
}