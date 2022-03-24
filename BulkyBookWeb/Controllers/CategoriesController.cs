using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Controllers
{
    public class CategoriesController : Controller
    {
        private BulkyDbContext _dbContext;
        public CategoriesController(BulkyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            IList<Category> categories = await RetrieveAllCategoriesAsync();

            return View(categories);
        }

        private async Task<IList<Category>> RetrieveAllCategoriesAsync()
        {
            IList<Category> categoriesList;
            try
            {
                categoriesList = await _dbContext.Categories.ToListAsync();
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
                return Ok(RetrieveAllCategoriesAsync());
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
                    await _dbContext.Categories.AddAsync(category);
                    await _dbContext.SaveChangesAsync();

                    TempData["success"] = $"Category {category.Name} created successfully";

                    return RedirectToAction("Index");
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
        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            try
            {
                /*var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(category => category.Id == id);*/
                var category = await _dbContext.FindAsync<Category>(id);
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
                var dbCategory = await _dbContext.FindAsync<Category>(id);
                if(dbCategory == null)
                {
                    return NotFound();
                }

                dbCategory.Name = category.Name;
                dbCategory.DisplayOrder = category.DisplayOrder;    

                await _dbContext.SaveChangesAsync();
                TempData["success"] = $"Category {dbCategory.Name} updated successfully";

                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                throw;
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                /*var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(category => category.Id == id);*/
                var category = await _dbContext.FindAsync<Category>(id);
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
                var dbCategory = await _dbContext.FindAsync<Category>(id);
                if(dbCategory != null)
                {
                    _dbContext.Remove(dbCategory);
                    await _dbContext.SaveChangesAsync();
                }
                TempData["success"] = $"Category {category.Name} deleted successfully";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

            return BadRequest();
        }
    }
}
