using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]//optional, at first, then mandatory on all other controllers once applied
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task<IEnumerable<Product>> ReturnAll()
        {
            var products = await _unitOfWork.ProductRepository.GetAll();

            return products;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await ReturnAll();
                if(products == null)
                {
                    return NotFound();
                }
                return View(products);
            }
            catch (Exception)
            {

                throw;
            } 
        }
        // GET: CoverTypeController
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await ReturnAll();
                if(products == null)
                {
                    return NotFound();
                }
                return View(products);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        // GET: CoverTypeController/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CoverTypeController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoverTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //injects a key/pair in the token that will be validated at every step of the request
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _unitOfWork.ProductRepository.Add(product);
                    await _unitOfWork.Save();

                    TempData["success"] = $"Product {product.Title} created successfully";

                    return RedirectToAction(nameof(Index));
                }

                return View(product);
            }
            catch
            {
                throw;
            }
        }

        // GET: CoverTypeController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            try
            {
                var dbProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(
                    prod => prod.Id == id);

                if(dbProduct == null)
                {
                    return NotFound(dbProduct);
                }

                return View(dbProduct);
                    
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // POST: CoverTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product productParam)
        {
            if(id == 0)
            {
                return NotFound();
            }
            try
            {
                if(productParam == null)
                {
                    return NotFound();
                }

                await _unitOfWork.ProductRepository.Update(productParam);
                
                await _unitOfWork.Save();

                TempData["success"] = $"Product has been successfully changed";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        // GET: CoverTypeController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.GetFirstOrDefault(
                prod => prod.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: CoverTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Product product)
        {
            if(id == 0)
            {
                return NotFound();
            }
            try
            {
                var dbProduct = await _unitOfWork.ProductRepository.GetAsync(id);

                if(dbProduct == null)
                {
                    return NotFound();
                }
                _unitOfWork.ProductRepository.Delete(dbProduct);
                await _unitOfWork.Save();

                TempData["success"] = $"Product {product.Title} deleted successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Upsert()//int id)
        {
            /*if (id == 0)
            {
                return NotFound();
            }

            var dbProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(
                prod => prod.Id == id);

            if(dbProduct == null)
            {
                return NotFound();
            }

            return View(dbProduct);*/

            //Filling up dropdown list (IEnumerable)
            IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll()
                .Result.Select(
                    cat => new SelectListItem
                    {
                        Text = cat.Name,
                        Value = cat.Id.ToString()
                    }
                );

            var coverTypeList = _unitOfWork.CoverTypeRepository.GetAll()
                .Result.Select(
                    ct => new SelectListItem
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString()
                    }
                );

            var productList = _unitOfWork.ProductRepository.GetAll()
                .Result.Select(
                    prod => new SelectListItem
                    {
                        Text = prod.Title,
                        Value = prod.Id.ToString()
                    }
                );

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(int id = 0, Product productParam = null!)
        {
            try
            {
                var dbProduct = await _unitOfWork.ProductRepository.GetAsync(id);
                //Id does not exist, Create Product
                if(dbProduct == null)
                {
                    await Create(productParam);
                }
                else//If existing, update product
                {
                    await Edit(dbProduct.Id, dbProduct);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
