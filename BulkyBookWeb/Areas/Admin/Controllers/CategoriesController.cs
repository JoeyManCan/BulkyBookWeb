using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulkyBook.DataAccess.Repositories;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]//optional, at first, then mandatory on all other controllers once applied
    public class CategoriesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await RetrieveAllCategoriesAsync();

            return View(categories);
        }

        private async Task<IEnumerable<Category>> RetrieveAllCategoriesAsync()
        {
            IEnumerable<Category> categoriesList;
            try
            {
                categoriesList = await _unitOfWork.CategoryRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw;
            }

            return categoriesList;
        }

        public async Task<IActionResult> GetAll()
        {
            var categories = await RetrieveAllCategoriesAsync();
            if(categories != null)
            {
                return Ok(categories);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //injects a key/pair in the token that will be validated at every step of the request
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if(category.Name == category.DisplayOrder.ToString())
                {
                    ModelState.AddModelError("name",
                        "The name cannot match the Display order");
                    ModelState.AddModelError("displayorder",
                        "The Display order cannot match the name");
                }
                if (ModelState.IsValid)
                {
                    await _unitOfWork.CategoryRepository.Add(category);
                    await _unitOfWork.Save();

                    TempData["success"] = $"Category {category.Name} created successfully";

                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception)
            {

                throw;
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            try
            {
                /*var category = await _categoryRepository.Categories
                    .FirstOrDefaultAsync(category => category.Id == id);*/
                var category = _unitOfWork.CategoryRepository.
                    GetFirstOrDefault(category => category.Id == id); 
                if(category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (category == null)
            {
                return NotFound();
            }

            try
            {
                var dbCategory = _unitOfWork.CategoryRepository.
                    GetFirstOrDefault(category => category.Id == id);
                if(dbCategory == null)
                {
                    return NotFound();
                }

                dbCategory.Name = category.Name;
                dbCategory.DisplayOrder = category.DisplayOrder;    

                _unitOfWork.CategoryRepository.Update(dbCategory);
                await _unitOfWork.Save();
                TempData["success"] = $"Category {dbCategory.Name} updated successfully";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {

                throw;
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                /*var category = await _categoryRepository.Categories
                    .FirstOrDefaultAsync(category => category.Id == id);*/
                var category = _unitOfWork.CategoryRepository
                    .GetFirstOrDefault(category => category.Id == id); 

                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception)
            {

                throw;
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Category category)
        {
            try
            {
                if(category == null)
                {
                    return NotFound();
                }
                var dbCategory = await _unitOfWork.CategoryRepository.GetAsync(id);
                if(dbCategory == null)
                {
                    return NotFound();
                }
                _unitOfWork.CategoryRepository.Delete(dbCategory);
                await _unitOfWork.Save();
                TempData["success"] = $"Category {category.Name} deleted successfully";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }

            return BadRequest();
        }
    }
}
