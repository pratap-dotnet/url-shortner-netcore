using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortnerNetCore.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UrlShortnerNetCore.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly ITableRepository<CounterEntity> counterRepository;
        private readonly ITableRepository<UrlEntity> urlRepository;

        public ApiController(ITableRepository<CounterEntity> counterRepository,
            ITableRepository<UrlEntity> urlRepository) 
        {
            this.counterRepository = counterRepository;
            this.urlRepository = urlRepository;
        }

        // POST api/values
        [HttpPost]
        [Route("shorten")]
        public async Task<string> Post([FromBody]UrlViewModel model)
        {   
            long number = 0;
            await counterRepository.Update(Constants.DefaultPartitionKey, Constants.DefaultRowKey,
                (existing) =>
                {
                    existing.Count++;
                    number = existing.Count;
                });    

            var encodedUrl = Base58Encoder.Encode(number);
            await urlRepository.Insert(new UrlEntity(encodedUrl, model.Url));
            return encodedUrl;
        }
        
    }
}
