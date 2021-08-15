using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Demo.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Logout()
		{
            return SignOut("Cookies", "oidc");
		}
        public async Task<IActionResult> CallApi()
		{

            //获取访问令牌
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            //创建HTTP客户端
            var client = new HttpClient();

            //设置授权请求头
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync("http://localhost:5002/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                ViewBag.IsBool = false;
                ViewBag.Json = "[]";
            }
            else
            {
                //转换API结果
                var content = await response.Content.ReadAsStringAsync();
                ViewBag.Json = JArray.Parse(content);
                ViewBag.IsBool = true;

            }
            return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
