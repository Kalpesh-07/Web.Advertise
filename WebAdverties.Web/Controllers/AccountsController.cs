using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAdverties.Web.Models.Accounts;

namespace WebAdverties.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoUserPool;
        public AccountsController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cognitoUserPool = cognitoUserPool;
        }
        // GET: AccountsController
        public async Task<IActionResult> SignUp()
        {
            var model = new SignUpModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            if (ModelState.IsValid)
            {
                var user = _cognitoUserPool.GetUser(signUpModel.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExist", "User with this mail already exist");
                    return View(signUpModel);
                }

                var createdUser = await ((CognitoUserManager<CognitoUser>)_userManager).CreateAsync(user, signUpModel.Password);
                if (createdUser.Succeeded) 
                {
                    RedirectToAction("Confirm");
                }
            }
            return View();
        }

        // GET: AccountsController
        public async Task<IActionResult> Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmModel confirmModel)
        {
            if (ModelState.IsValid)
            {
                var user = await ((CognitoUserManager<CognitoUser>)_userManager).FindByEmailAsync(confirmModel.Email);
                if(user == null)
                {
                    ModelState.AddModelError("UserNotFound", "User with this mail not found");
                    return View(confirmModel);
                }

                var result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(user, confirmModel.Code,true).ConfigureAwait(false); ;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    return View(confirmModel);
                }
            }
            return View(confirmModel);
        }

        [HttpGet]
        public IActionResult Login(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe, false).ConfigureAwait(false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("LoginError", "Email and password do not match");
            }

            return View("Login", model);
        }
    }
}
