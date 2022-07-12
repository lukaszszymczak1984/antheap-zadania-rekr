using Microsoft.AspNetCore.Mvc;
using NipSearch.Db;
using NipSearch.Entities;
using NipSearch.Models;
using System.Diagnostics;

namespace NipSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISubjectsRepository _subjectsRepository;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, ISubjectsRepository subjectsRepository)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _subjectsRepository = subjectsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Pobieranie danych na postawie podanego numeru NIP
        /// </summary>
        /// <param name="nip"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSearchResult(string nip)
        {
            // utworzenie klienta odpowiedzialnego za pobranie informacji z API
            NipApiClient nipApiClient = new NipApiClient(_httpClientFactory);

            // pobranie danych z API
            ResponseEntity model = await nipApiClient.Get(nip);

            // pobranie danych testowych
            //ResponseEntity model = nipApiClient.GetTestDatas();

            // jeśli przedsiębiorca został znaleziony, zapisanie go w bazie danych
            if (model.Subject != null)
			{
                _subjectsRepository.SaveSubject(model.Subject);
            }

            // zwrócenie widoku z danymi
            return PartialView("_SearchResultPartial", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}