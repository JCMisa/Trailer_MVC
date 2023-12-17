using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Trailer.DataAccess.Repository.IRepository;
using Trailer.Models;

namespace TrailerWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Movie> movieList = _unitOfWork.Movie.GetAll(includeProperties: "Category");
            return View(movieList);
        }

		public async Task<IActionResult> Details(int? id)
		{
            if(id == null || id == 0)
            {
                return NotFound();
            }
            else
            {
				Movie? movie = await _unitOfWork.Movie.GetAsync(u => u.Id == id, includeProperties: "Category");
				return View(movie);
			}			
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}