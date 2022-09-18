using Project.BLL.DesingPatterns.GenericRepository.ConcRep;
using Project.ENTITIES.Entities;
using Project.MVCUI.Areas.Admin.AdminVMClasses;
using Project.MVCUI.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProdcutRepository _pRep;
        CategoryRepository _cRep;

        public ProductController()
        {
            _pRep = new ProdcutRepository();
            _cRep = new CategoryRepository();

        }
        // GET: Admin/Product
        public ActionResult ProductList(int? id)
        {
            ProductVM pvm = new ProductVM
            {
                Products = id == null ? _pRep.GetAll() : _pRep.Where(x => x.CategoryID == id)
            };
            return View(pvm);
        }

        public ActionResult AddProduct()
        {
            ProductVM pvm = new ProductVM()
            {
                Categories = _cRep.GetActives()
            };

            return View(pvm);
        }


        [HttpPost]
        public ActionResult AddProduct(Product product, HttpPostedFileBase image, string fileName)
        {
            product.ImagePath = ImageUploader.UploaderImage("/Pictures/", image, fileName);
            _pRep.Add(product);
            return RedirectToAction("ProductList");
        }

        public ActionResult UpdateProduct(int id)
        {
            ProductVM pvm = new ProductVM
            {
                Product = _pRep.Find(id),
                Categories = _cRep.GetActives()
            };
            return View(pvm);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {


            _pRep.Updated(product);
            return RedirectToAction("ProductList");
        }

        public ActionResult DeleteProduct(int id)
        {
            _pRep.Deleted(_pRep.Find(id));
            return RedirectToAction("ProductList");
        }
    }
}