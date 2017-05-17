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
        Amazon.Extensions.NETCore.Setup.AWSOptions _ops;

        public HomeController(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb, Amazon.Extensions.NETCore.Setup.AWSOptions ops)
        {
            _dynamoDb = dynamoDb;
            _ops = ops;
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
            Amazon.Runtime.AWSCredentials creds = null;
            if (_ops != null)
            {
                if(_ops.Credentials != null)
                {
                    creds = _ops.Credentials;
                }
                else
                if(!string.IsNullOrEmpty(_ops.Profile) && !string.IsNullOrEmpty(_ops.ProfilesLocation))
                {
                    Amazon.Runtime.CredentialManagement.CredentialProfile basicProfile;
                    var sharedFile = new Amazon.Runtime.CredentialManagement.SharedCredentialsFile(_ops.ProfilesLocation);
                    if (sharedFile.TryGetProfile(_ops.Profile, out basicProfile))                     
                    {
                        creds =  Amazon.Runtime.CredentialManagement.AWSCredentialsFactory.GetAWSCredentials(basicProfile, sharedFile);
                    }
                }
            }
            
            creds = creds ??  Amazon.Runtime.FallbackCredentialsFactory.GetCredentials();
            
           
            ViewData["Message"] = creds?.GetCredentials().AccessKey;      

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View(new AspNet.Security.OpenIdConnect.Primitives.OpenIdConnectRequest {
                GrantType = "password"
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
