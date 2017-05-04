using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WelthPeck.Controllers
{
    public class HomeController : Controller
    {
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;

        public HomeController(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var tables = await _dynamoDb.ListTablesAsync();
            ViewData["Message"] = string.Join(", ", tables.TableNames);

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
