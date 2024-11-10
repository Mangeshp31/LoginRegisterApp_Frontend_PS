using LoginRegisterApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Security;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoginRegisterApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri baseAddress = new Uri("https://localhost:7298/api");
        private readonly HttpClient _client;

        

        public HomeController()
        {
            //ssl crtfct
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            //new added code

            _client = new HttpClient(clientHandler);
            _client.BaseAddress = baseAddress;
        }
       

        //Index Page
        /// <summary>
        /// This is the index page, this is the first page which displays to the user on execution of the application.
        /// </summary>
        /// <returns>View(Index)</returns>
        public IActionResult Index()
        {
            return View();
        }


        //Registration page view
        /// <summary>
        /// It is registration page, which is responsible to show the registration window to the user.
        /// </summary>
        /// <returns>View(Create)</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        //Registration method
        /// <summary>
        /// This method is responsible for consuming Registration Api. It will take the data from user which will
        /// be type of UserRegistration cls,and convert that data into serilized object and will perform registration.
        /// </summary>
        /// <param name="model">model(UserRegistration)</param>
        /// <returns>View(Success/Error)</returns>
        [HttpPost]
        public IActionResult Create(UserRegistration model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Authentication/Register", content).Result;

            if(response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "User Successfully Registered";
                return RedirectToAction("AfterRegistrationPage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User already Registered");
            }
            return View();
        }


        //After Registration page
        /// <summary>
        /// After successful registration user will be redirected to this page where he/she can redirect to login page.
        /// </summary>
        /// <returns>View(AfterRegistrationPage)</returns>
        [HttpGet]
        public IActionResult AfterRegistrationPage()
        {
            return View(AfterRegistrationPage);
        }


        //Login Page view
        /// <summary>
        /// It is a Login page, which is  responsible to show the login window to the user.
        /// </summary>
        /// <returns>View(Create)</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        //UserLogin
        /// <summary>
        /// This method is responsible for consuming Login Api. It will take the data from user which will
        /// be type of UserLogin cls,and and check the logic and will perform Login.
        /// </summary>
        /// <param name="model">model(UserLogin)</param>
        /// <returns>View(Success/Error)</returns>
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var result = await AuthenticateUserAsync(userLogin);
                if (result)
                {
                    ViewData["UserName"] = userLogin.Username;
                    ViewData["Password"] = userLogin.Password;
                    return View("UserHome");

                    //return RedirectToAction("UserDeshboard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName, Password");
                }
            }
            //return View("");
            //return ViewBag["Invalid UserName Password"];
            //ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View();

        }
        private async Task<bool> AuthenticateUserAsync(UserLogin userLogin)
        {
            try
            {
                string data = JsonConvert.SerializeObject(userLogin);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Authentication/Login", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        //UserHome Page
        /// <summary>
        /// It is User Home page which is responsible to show the user details.
        /// </summary>
        /// <returns>View(Create)</returns>
        [HttpGet]
        public IActionResult UserDeshboard()
        {

            return View();
        }



        //LogOut
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}




//public HomeController(ILogger<HomeController> logger)
//{
//    _logger = logger;
//}

//public IActionResult Privacy()
//{
//    return View();
//}

//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//public IActionResult Error()
//{
//    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//}