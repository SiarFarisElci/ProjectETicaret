using Project.BLL.DesingPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class RegisterController : Controller
    {
        AppUserRepository _apRep;
        ProfileRepository _pRep;

        public RegisterController()
        {
            _apRep = new AppUserRepository();
            _pRep = new ProfileRepository();
        }

        // GET: Register
        public ActionResult RegisterNow()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterNow(AppUser appUser , Profile profile)
        {
            appUser.Password = DantexCrypt.Crypt(appUser.Password);//Şifreyi kriptoladık

            if (_apRep.Any(x=> x.UserName == appUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanıcı ismi daha önce alınmıs";
                return View();
            }
            else if (_apRep.Any(x=> x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "Email zaten Kayıtlı";
                return View();
            }

            //Kullanıcı başarılı bir şekilde kayıt olduktan sonra email gonder 

            string gonderilecekMail = "Tebrikler...Hesabınız olusturulmustur...Hesabınızı aktive etmek https://localhost:44306/Register/Activation/" + appUser.ActivationCode + " linkine tıklayabilirsiniz";

            MailService.Send(appUser.Email, body: gonderilecekMail, subject: "Hesab Aktivasyon");
            _apRep.Add(appUser);

            if (!string.IsNullOrEmpty(profile.FirstName.Trim()) || !string.IsNullOrEmpty(profile.LastName.Trim()))
            {
                profile.ID = appUser.ID;
                _pRep.Add(profile);
            }


            return View("RegisterOk");
        }

        public ActionResult Activation(Guid id)
        {
            AppUser aktifEdilecek = _apRep.FirstOrDefault(x => x.ActivationCode == id);

            if (aktifEdilecek != null)
            {
                aktifEdilecek.Active = true;
                _apRep.Updated(aktifEdilecek);
                TempData["HesapAktifMi"] = "Hesabınız Aktif Hale getirildi";

                return RedirectToAction("Login", "Home");
            }



            return RedirectToAction("Login" ,"Home");
        }

        public ActionResult RegisterOK()
        {
            return View();
        }
    }
}