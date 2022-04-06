using BulkyBook.DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CoverTypesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CoverTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var coverTypes = await _unitOfWork.CoverTypeRepository.GetAll();
                return View(coverTypes);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
