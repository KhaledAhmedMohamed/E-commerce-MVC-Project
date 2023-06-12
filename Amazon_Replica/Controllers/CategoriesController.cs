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
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepo categoryRepo;

        public CategoriesController(ICategoryRepo _category)
        {
            categoryRepo = _category;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return categoryRepo.GetAll() != null ? 
                          View(categoryRepo.GetAll()) :
                          Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || categoryRepo.GetAll() == null)
            {
                return NotFound();
            }

            var category = categoryRepo.GetDetails(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Image")] [FromForm]Category category)
        {
            if (ModelState.IsValid)
            {
                Category newCategory = new Category()
                {
                    Name = category.Name,
                    Description = category.Description
                };

                if (Request.Form.Files.FirstOrDefault() != null)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        newCategory.Image = stream.ToArray();
                    }
                }

                categoryRepo.Insert(newCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || categoryRepo.GetAll() == null)
            {
                return NotFound();
            }

            var category = categoryRepo.GetDetails(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Image")] [FromForm]Category category)
        {
            if (id != category.Id)
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
                        category.Image = stream.ToArray();
                    }
                }

                try
                {
                    categoryRepo.UpdateCategory(id,category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || categoryRepo.GetAll() == null)
            {
                return NotFound();
            }

            var category = categoryRepo.GetDetails(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (categoryRepo.GetAll() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = categoryRepo.GetDetails(id);
            if (category != null)
            {
                categoryRepo.DeleteCategory(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (categoryRepo.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
