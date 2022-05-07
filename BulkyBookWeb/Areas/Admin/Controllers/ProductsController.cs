using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]//optional, at first, then mandatory on all other controllers once applied
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        //private readonly ILogger<ProductsController> _logger;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;

        }

        private async Task<IEnumerable<Product>> ReturnAll()
        {
            var products = await _unitOfWork.ProductRepository.GetAll();

            return products;
        }
        private IEnumerable<Product> ReturnAll(string? includeProperties = null)
        {
            var products =  _unitOfWork.ProductRepository.GetAll(includeProperties);

            return products;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await ReturnAll();
                if (products == null)
                {
                    return NotFound(products);
                }
                return View(products);
            }
            catch (Exception)
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

                await _unitOfWork.ProductRepository.Add(product);
                await _unitOfWork.Save();

                TempData["success"] = $"Product {product.Title} created successfully";

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                throw;
            }
        }

        // GET: CoverTypeController/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            try
            {
                var dbProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(
                    prod => prod.Id == id);

                if (dbProduct == null)
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
            if (id == 0)
            {
                return NotFound();
            }
            try
            {
                if (productParam == null)
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
        public IActionResult Delete(int id)
        {
            if (id == 0)
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
            if (id == 0)
            {
                return NotFound();
            }
            try
            {
                var dbProduct = await _unitOfWork.ProductRepository.GetAsync(id);

                if (dbProduct == null)
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

        private ProductViewModel InstantiatePVM()
        {
            ProductViewModel productViewModel = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.CategoryRepository
                    .ReturnSelectListItems(),
                CoverTypeList = _unitOfWork.CoverTypeRepository
                    .ReturnSelectListItems()

            };
            return productViewModel;

        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int id)
        {

            var productViewModel = InstantiatePVM();


            if (id == 0)
            {
                //ViewBag.CategoryList = categoryList;
                //ViewData["coverTypeList"] = coverTypeList;
                return View(productViewModel);
            }

            /*var dbProduct = await _unitOfWork.ProductRepository.GetFirstOrDefault(
                prod => prod.Id == id);

            if(dbProduct == null)
            {
                return NotFound();
            }*/

            return View(productViewModel);

            //return View();
        }

        private string SetFileUploadUrl(IFormFile formFile)
        {
            if (formFile != null)
            {
                //avoinding more than one file with the same name
                var fileName = Guid.NewGuid().ToString();
                var filePath = @"images\products";
                var folderLocation = Path.Combine(_hostEnvironment.WebRootPath,
                    filePath);
                var fileExtension = Path.GetExtension(formFile.FileName);
                var fullPath = fileName + fileExtension;

                using (var fileStreams = new FileStream(
                    Path.Combine(folderLocation, fullPath),
                    FileMode.Create))
                {
                    formFile.CopyTo(fileStreams);

                }

                return @$"{filePath}\{fullPath}";
            }

            return string.Empty;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductViewModel productViewModel, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productViewModel.Product.ImageUrl = SetFileUploadUrl(formFile);

                    var dbProduct = await _unitOfWork
                    .ProductRepository.GetAsync(productViewModel.Product.Id);
                    //Id does not exist, Create Product
                    if (dbProduct == null)
                    {
                        await Create(productViewModel.Product);
                    }
                    else//If existing, update product
                    {
                        await Edit(dbProduct.Id, dbProduct);
                    }

                    return RedirectToAction(nameof(Index));
                }
                return View(InstantiatePVM());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        #region API CALLS 

        // GET: CoverTypeController
        public IActionResult GetAll()
        {
            try
            {
                var products = ReturnAll("Category,CoverType");
                if (products == null)
                {
                    return NotFound();
                }
                return Json(new {data = products});
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
