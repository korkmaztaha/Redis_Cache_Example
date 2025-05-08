using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text;


namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        //Distributed Cache
        //Cache verisi harici bir sistemde(Redis, SQL Server, vs.) saklanır.
        //Tüm uygulama sunucuları aynı veriye erişebilir — bu nedenle yük dengeleme(load balancing) senaryolarına uygundur.
        //Veriler uygulama kapanınca kaybolmaz (Redis gibi sistemlerde).
        //Örnek: IDistributedCache(Redis ile kullanılırsa güçlü ve ölçeklenebilir olur)
        //Çoklu sunucu ortamları ve yüksek erişilebilirlik için tercih edilir.
        readonly IDistributedCache _distributedCache;
        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("name");
            //GetAsync Binary olarak döner
            var surnameBinary = await _distributedCache.GetAsync("surname");
            var surname = Encoding.UTF8.GetString(surnameBinary);
            return Ok(new
            {
                name,
                surname
            });
        }
    }
}