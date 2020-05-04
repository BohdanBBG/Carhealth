using Carhealth.Models;
using Carhealth.Models.IdentityModels;
using Carhealth.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password); // пользователь  добавляется в базу данных
                                                                                   // В качестве параметра передается сам пользователь и его пароль.
                                                                                   // Данный метод возвращает объект IdentityResult,
                                                                                   // с помощью которого можно узнать успешность выполненной операции

                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false); // устанавливаем аутентификационные куки для добавленного пользователя
                                                                   // В этот метод передается объект пользователя, который аутентифицируется, 
                                                                   // и логическое значение, указывающее, надо ли сохранять куки в течение продолжительного времени

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description); //Если добавление прошло неудачно, то добавляем к состоянию модели с помощью метода
                                                                                   // ModelState все возникшие при добавлении ошибки, и отправленная модель возвращается в представление.
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) // получаем адрес для возврата в виде параметра returnUrl и передаем его в модель LoginViewModel
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) // получаем данные из представления в виде модели LoginViewModel
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false); // выполняет аутентификации пользователя 
                                                                                                                             //Этот метод принимает логин и пароль пользователя. 
                                                                                                                             // Третий параметр метода указывает, 
                                                                                                                             // надо ли сохранять устанавливаемые куки на долгое время.

                if (result.Succeeded) //Если аутентификация завершилось успешно, то используем свойство ReturnUrl модели LoginViewModel для возврата пользователя на предыдущее место
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)) // удостовериться, что адрес возврата принадлежит приложению с помощью метода Url.IsLocalUrl(). 
                                                                                                   // Это позволит избежать перенаправлений на нежелательные сайты.
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else // Если же адрес возврата не установлен или не принадлежит приложению, выполняем переадресацию на главную страницу.
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }

            return View(model);
        }


        [HttpGet]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() // выполняет выход пользователя из приложения
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync(); // очищает аутентификационные куки.

            return RedirectToAction("Login","Account");
        }

    }
}
