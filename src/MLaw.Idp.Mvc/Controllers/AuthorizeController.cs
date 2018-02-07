using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MLaw.Idp.Mvc.Models;
using MLaw.Idp.Mvc.Services;

namespace MLaw.Idp.Mvc.Controllers
{

    public class AuthorizeController : Controller
    {

        private readonly ILoggedUsersStorage _loggedUsersStorage;
        private readonly ILogger<AuthorizeController> _logger;

        public AuthorizeController(
            ILoggedUsersStorage loggedUsersStorage,
            ILogger<AuthorizeController> logger)
        {

            _loggedUsersStorage = loggedUsersStorage;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IQueryCollection query = Request.Query;
            string state = query["state"].FirstOrDefault();
            string redirectUrl = query["redirectUrl"].FirstOrDefault();
            LogingViewModel logingViewModel = new LogingViewModel
            {
                RedirectUrl = redirectUrl,
                State = state,
                UserName = null,
                ValidationCode = null
            };
            return View(logingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogingViewModel loginViewModel)
        {
            // validate login
            if (loginViewModel.ValidationCode != "code")
            {
                ModelState.AddModelError("Validation Code", "Validation code is not valid. Use 'code' for Validation Code :-)");
                return View("Index",loginViewModel);
            }

            // username must uniquely identify user. i use email in place of username
            string cacheKey = await _loggedUsersStorage.SaveLoginAsync(LoggedinUserModel.Create(loginViewModel.Email,loginViewModel.Name,loginViewModel.Email));

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"code", cacheKey},
                {"state", loginViewModel.State}
            };
            string redirectUrl = QueryHelpers.AddQueryString(loginViewModel.RedirectUrl, parameters);
            return Redirect(redirectUrl);
        }


    }
}