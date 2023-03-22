using Client.Models;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileService _fileService;

        public HomeController(ILogger<HomeController> logger, IFileService fileService)
        {
            _fileService = fileService;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
            _fileService.DivideAndSendFile();

            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm] IFormFile file)
        {
            Debug.WriteLine(file?.FileName);

            return View();
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