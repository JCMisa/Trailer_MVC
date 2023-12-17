using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using Trailer.DataAccess.Repository.IRepository;
using Trailer.Models;
using Trailer.Models.ViewModels;
using Trailer.Utility;

namespace TrailerWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class MovieController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironmemnt;
        public MovieController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironmemnt)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironmemnt = webHostEnvironmemnt;
        }






        public IActionResult Index()
        {
            List<Movie> movieList = _unitOfWork.Movie.GetAll(includeProperties:"Category").ToList();            
            return View(movieList);
        }







        [HttpGet]
        public async Task<IActionResult> Upsert(int? id) //Update and Insert
        {
            MovieVM movieVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Movie = new Movie()
            };

            if(id == null || id == 0)
            {
                //Create
                return View(movieVM);
            }
            else
            {
                //Update
                movieVM.Movie = await _unitOfWork.Movie.GetAsync(u => u.Id == id);
                return View(movieVM);
            }
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert([Bind("Movie, CategoryList")] MovieVM movieVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironmemnt.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string moviePath = Path.Combine(wwwRootPath, @"images\movie");

                    if(!string.IsNullOrEmpty(movieVm.Movie.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = 
                            Path.Combine(wwwRootPath, movieVm.Movie.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using ( var fileStream = new FileStream(Path.Combine(moviePath, fileName), FileMode.Create) )
                    {
                        file.CopyTo(fileStream);
                    }

                    movieVm.Movie.ImageUrl = @"\images\movie\" + fileName;
                }

                if(movieVm.Movie.Id == 0)
                {
                    await _unitOfWork.Movie.AddAsync(movieVm.Movie);
					TempData["success"] = "Category created successfully"; //for toaster before redirecting
				}   
                else
                {
                    _unitOfWork.Movie.Update(movieVm.Movie);
					TempData["success"] = "Category updated successfully"; //for toaster before redirecting
				}

                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                movieVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(movieVm);
            }
        }





        /*
        //GET - Edit
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Movie? movie = await _unitOfWork.Movie.GetAsync(u => u.Id == id);

        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(movie);
        //}





        //POST - Edit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int? id, [Bind("Id, Title, Description, Director, ListPrice, Price, Price50, Price100")] Movie movie)
        //{
        //    if (id != movie.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Movie.Update(movie);
        //        _unitOfWork.SaveAsync();
        //        TempData["success"] = "Category updated successfully"; //for toaster before redirecting
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(movie);
        //}
        */






        /*
        //GET - Delete
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Movie? movie = await _unitOfWork.Movie.GetAsync(u => u.Id == id);

        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(movie);
        //}






        //POST - Delete
        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeleteMovie(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    if (_unitOfWork == null)
        //    {
        //        return Problem("Entity set 'IUnitOfWork'  is null.");
        //    }

        //    Movie? movie = await _unitOfWork.Movie.GetAsync(u => u.Id == id);

        //    if (movie != null)
        //    {
        //        _unitOfWork.Movie.Remove(movie);
        //    }

        //    await _unitOfWork.SaveAsync();
        //    TempData["success"] = "Category deleted successfully"; //for toaster before redirecting
        //    return RedirectToAction(nameof(Index));
        //}
        */






        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Movie> movieList = _unitOfWork.Movie.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = movieList });
        }




        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productToBeDeleted = await _unitOfWork.Movie.GetAsync(u => u.Id == id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }

            var oldImagePath = 
                Path.Combine(_webHostEnvironmemnt.WebRootPath, 
                productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Movie.Remove(productToBeDeleted);
            await _unitOfWork.SaveAsync();
			TempData["success"] = "Category deleted successfully"; //for toaster before redirecting
			return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
