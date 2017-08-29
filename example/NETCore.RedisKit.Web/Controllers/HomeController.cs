using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NETCore.RedisKit.Web.Controllers
{
    public class HomeController:Controller
    {
        private readonly IRedisService _RedisService;
        private readonly ILogger _Logger;
        public HomeController(IRedisService redisService, ILogger<HomeController> logger)
        {
            _RedisService = redisService;
            _Logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                _RedisService.ItemSet("hello", "world");
            }
            catch (Exception ex)
            {
                _Logger.LogError(1, ex, "设置Redis缓存失败");
            }


            return View();
        }

        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";

            try
            {
                ViewData["Hello"] = _RedisService.ItemGet<string>("hello");
            }
            catch (Exception ex)
            {

                _Logger.LogError(2, ex, "获取Redis缓存失败");
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
