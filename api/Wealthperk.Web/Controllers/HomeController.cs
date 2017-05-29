using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wealthperk.Model;
using Wealthperk.Web.ViewModels;

namespace WelthPeck.Controllers
{
    public class HomeController : Controller
    {
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        Amazon.Extensions.NETCore.Setup.AWSOptions _ops;
        private IRiskStrategyConfiguration _risks;

        public HomeController(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb, Amazon.Extensions.NETCore.Setup.AWSOptions ops, IRiskStrategyConfiguration risks)
        {
            _dynamoDb = dynamoDb;
            _ops = ops;
            _risks = risks;
        }

        public IActionResult Index()
        {
            if (Request.Query.ContainsKey("success")) {
                ViewData["Success"] = "Operation completed successfully!";
            } else if (Request.Query.ContainsKey("error")){
                string errors = Request.Query["error"];
                if (!string.IsNullOrEmpty("errors")){
                    var messages = errors.Split('|');
                    ViewBag.Errors = messages;
                    ViewData["Error"] = "Operation failed with errors.";
                }
            }
            return View();
        }

        public async Task<IActionResult> About()
        {
            var cancelSource = new CancellationTokenSource();
            cancelSource.CancelAfter(1000);
            try{
                var tables = await _dynamoDb.ListTablesAsync(cancelSource.Token);
                ViewData["Message"] = "Dynamo Db functioning properly";
            }
            catch (TaskCanceledException ex){
                ViewData["ErrorMessage"] = "Dynamo Db is NOT functioning";
            }
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Accounts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Settings()
        {
            ViewData["RiskStrategies"] = _risks.RiskStrategies.Keys.ToArray();
            return View();
        }

        public IActionResult Login()
        {
            return View(new AspNet.Security.OpenIdConnect.Primitives.OpenIdConnectRequest {
                GrantType = "password"
            });
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Contact(ContactRequest request)
        {
            await Task.Yield();
            return StatusCode(200);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
