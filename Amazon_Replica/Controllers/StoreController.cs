using Amazon_Replica.Data;
using Amazon_Replica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Amazon_Replica.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductRepo productRepo;
        private readonly ICategoryRepo categoryRepo;

        public StoreController(IProductRepo _productRepo, ICategoryRepo _categoryRepo)
        {
            productRepo = _productRepo;
            categoryRepo = _categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            SelectList catList = new SelectList(categoryRepo.GetAll(),"Id", "Name");
            ViewBag.categories = catList;
            return productRepo.GetAll() != null ? View(productRepo.GetAll()) : Problem("Entity set 'AmazonContext.Products'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || productRepo.GetAll() == null)
            {
                return NotFound();
            }

            var product = productRepo.GetDetails(id); 
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> GetByName(string name)
        {
            SelectList catList = new SelectList(categoryRepo.GetAll(), "Id", "Name");
            ViewBag.categories = catList;
            return productRepo.SearchByName(name) != null ? View("Index", productRepo.SearchByName(name)) : Problem("Entity set 'AmazonContext.Products'  is null.");
        }

        public async Task<IActionResult> GetByCategory(string id)
        {
            SelectList catList = new SelectList(categoryRepo.GetAll(), "Id", "Name");
            ViewBag.categories = catList;
            return productRepo.GetProductByCategoryId(int.Parse(id)) != null ? View("Index", productRepo.GetProductByCategoryId(int.Parse(id))) : Problem("Entity set 'AmazonContext.Products'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> GetByPriceRange(string minPrice, string maxPrice)
        {
            SelectList catList = new SelectList(categoryRepo.GetAll(), "Id", "Name");
            ViewBag.categories = catList;
            return productRepo.SearchByPriceRange(minPrice,maxPrice) != null ? View("Index", productRepo.SearchByPriceRange(minPrice, maxPrice)) : Problem("Entity set 'AmazonContext.Products'  is null.");
        }

    }
}
