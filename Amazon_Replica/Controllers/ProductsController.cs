using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Amazon_Replica.Data;
using Amazon_Replica.Models;
using Amazon_Replica.Services;
using Microsoft.AspNetCore.Authorization;

namespace Amazon_Replica.Controllers
{
    [Authorize(Roles ="Admin,Moderator")]
    public class ProductsController : Controller
    {
        private readonly IProductRepo productRepo;
        private readonly ICategoryRepo categoryRepo;

        public ProductsController(IProductRepo context, ICategoryRepo context2)
        {
            productRepo = context;
            categoryRepo = context2;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = productRepo.GetAll();
            return View(applicationDbContext);
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(categoryRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,NumInStock,Image,CategoryId")] [FromForm]Product product)
        { 
            if (ModelState.IsValid)
            {
                Product newProduct = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    NumInStock = product.NumInStock,
                };
                if (Request.Form.Files.FirstOrDefault() != null)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        newProduct.Image = stream.ToArray();
                    }
                }
                productRepo.Insert(newProduct);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(categoryRepo.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["CategoryId"] = new SelectList(categoryRepo.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,NumInStock,Image,CategoryId")] [FromForm]Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Request.Form.Files.FirstOrDefault() != null)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        product.Image = stream.ToArray();
                    }
                }
                try
                {
                    productRepo.UpdateProduct(id,product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(categoryRepo.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (productRepo.GetAll() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = productRepo.GetDetails(id);
            if (product != null)
            {
                productRepo.DeleteProduct(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (productRepo.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
