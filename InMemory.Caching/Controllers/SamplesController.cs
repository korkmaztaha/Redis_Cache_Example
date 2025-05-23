﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace InMemory.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {

        //In-Memory Cache
        //Veriler uygulama sunucusunun RAM'inde tutulur.
        //Tek bir sunucuya özeldir, ölçeklendirme (load balancing) durumunda diğer sunucular veriye erişemez.
        //Çok hızlıdır, ancak uygulama yeniden başlatıldığında cache temizlenir.
        //Örnek: IMemoryCache(ASP.NET Core'da yerleşik)
        // Hızlı ama tek sunuculuk çözümler için idealdir.


        readonly IMemoryCache _memoryCache;

        public SamplesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }



        [HttpGet("setName/{name}")]
        public void SetName(string name)
        {
            _memoryCache.Set("name", name);

        }

        [HttpGet("getName")]
        public string GetName()
        {
            //datanın bo gelme ihtilamine karşın önlem
            if (_memoryCache.TryGetValue<string>("name", out string name)) ;
            {
                return name.Substring(3);
            }
            //return _memoryCache.Get<string>("name");
        }
        //AbsoluteExpiration: verinin belirtilen sürenin sonunda silinir
        //SlidingExpiration :  belirtilen süre içinde veriye erişildiğinde süre tekrar başlar
        [HttpGet("setDate")]
        public void setDate()
        {

            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)

            });
        }
        [HttpGet("getDate")]
        public DateTime getDate()
        {
            return _memoryCache.Get<DateTime>("date");
        }

    }
}
