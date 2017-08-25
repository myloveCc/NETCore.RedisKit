using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NETCore.RedisKit.Loging;

namespace NETCore.RedisKit.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRedisService _RedisService;
        public HomeController(IRedisService redisService)
        {
            _RedisService = redisService;
        }

        public IActionResult Index()
        {
            _RedisService.StringSet("hello", "world");
            return View();
        }

        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";
            ViewData["Hello"] = _RedisService.StringGet<string>("hello");
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
