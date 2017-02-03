using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortnerNetCore.Models;

namespace UrlShortnerNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITableRepository<UrlEntity> urlRepository;
        public HomeController(ITableRepository<UrlEntity> urlRepository)
        {
            this.urlRepository = urlRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{url}")]
        public async Task<IActionResult> Index(string url)
        {
            var urlEntity = await urlRepository.GetByPartitionKeyAndRowKey(Constants.DefaultPartitionKey, url);
            if (urlEntity != null)
                return Redirect(urlEntity.Url);

            return NotFound();  
        }
    }
}
