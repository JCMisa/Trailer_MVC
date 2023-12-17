using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trailer.DataAccess.Data;
using Trailer.DataAccess.Repository.IRepository;
using Trailer.Models;
using Trailer.Utility;

namespace TrailerWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }










        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }










        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DisplayOrder")] Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name cannot exactly match the Display Order");
            }
            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.AddAsync(category);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Category created successfully"; //for toaster before redirecting
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }










        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = await _unitOfWork.Category.GetAsync(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id, Name, DisplayOrder")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.SaveAsync();
                TempData["success"] = "Category updated successfully"; //for toaster before redirecting
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }







        //GET - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = await _unitOfWork.Category.GetAsync(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (_unitOfWork == null)
            {
                return Problem("Entity set 'IUnitOfWork'  is null.");
            }
            Category? category = await _unitOfWork.Category.GetAsync(u => u.Id == id);
            if (category != null)
            {
                _unitOfWork.Category.Remove(category);
            }

            await _unitOfWork.SaveAsync();
            TempData["success"] = "Category deleted successfully"; //for toaster before redirecting
            return RedirectToAction(nameof(Index));
        }
    }
}
