using LogInMVC.DAL;
using LogInMVC.Models;
using LogInMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LogInMVC.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult LogIn(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                UsuarioDAL dal = new UsuarioDAL();
                Usuario user = dal.GetByLogIn(model.Username, model.Password);  // if user is null it doesn't exists in the DB

                // Validate user
                if (user != null)
                {
                    // Authentication successful
                    HttpContext.Session.SetString("Username", model.Username); // Set new session
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            }
            // Return to same view if not successful log-in
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                UsuarioDAL dal = new UsuarioDAL();
                Usuario user = new Usuario();

                user.UserName = model.UserName;

                if (dal.GetCountByUserName(model.UserName) > 0) 
                {
                    ModelState.AddModelError("", "Nombre de usuario existente");
                    return View(model);
                }

                dal.InsertBySignUp(user, model.Password);

                Usuario validateInsert = dal.GetByLogIn(model.UserName, model.Password);
                if (validateInsert != null)
                {
                    HttpContext.Session.SetString("Username", user.UserName);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "No se ha podido crear usuario");
            }

            return View(model);
        }
    }
}
