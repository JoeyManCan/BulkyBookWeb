using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CoverTypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CoverTypesController> _logger;

        public CoverTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task<IEnumerable<CoverType>> ReturnAll()
        {
            var coverTypes = await _unitOfWork.CoverTypeRepository.GetAll();

            return coverTypes;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var coverTypes = await ReturnAll();
                if(coverTypes == null)
                {
                    return NotFound();
                }
                return View(coverTypes);
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
                var coverTypes = await ReturnAll();
                if(coverTypes == null)
                {
                    return NotFound();
                }
                return View(coverTypes);
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
        public async Task<IActionResult> Create(CoverType coverType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _unitOfWork.CoverTypeRepository.Add(coverType);
                    await _unitOfWork.Save();

                    TempData["Success"] = $"Cover Type{coverType.Name} created successfully";

                    return RedirectToAction(nameof(Index));
                }

                return View(coverType);
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
                var coverType = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(
                    ct => ct.Id == id);

                if(coverType == null)
                {
                    return NotFound(coverType);
                }

                return View(coverType);
                    
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // POST: CoverTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoverType coverType)
        {
            if(id == 0)
            {
                return NotFound();
            }
            try
            {
                var dbCoverType = await _unitOfWork.CoverTypeRepository.GetFirstOrDefault(
                    ct => ct.Id == id);
                if(dbCoverType == null)
                {
                    return NotFound();
                }

                dbCoverType.Name = coverType.Name;
                
                _unitOfWork.CoverTypeRepository.Update(dbCoverType);
                await _unitOfWork.Save();

                TempData["Success"] = $"Cover Type has been successfully changed to {coverType.Name}";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        // GET: CoverTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var coverType = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(
                ct => ct.Id == id);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: CoverTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CoverType coverType)
        {
            if(id == 0)
            {
                return NotFound();
            }
            try
            {
                var dbCoverType = await _unitOfWork.CoverTypeRepository.GetAsync(id);

                if(dbCoverType == null)
                {
                    return NotFound();
                }
                _unitOfWork.CoverTypeRepository.Delete(dbCoverType);
                await _unitOfWork.Save();

                TempData["success"] = $"Cover Type {coverType.Name} deleted successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
