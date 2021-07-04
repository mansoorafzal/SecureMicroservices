using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Movies.Client.Services;

namespace Movies.Client.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {   
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        public async Task<IActionResult> Index()
        {
            await LogTokenAndClaims();
            return View(await _movieService.GetMovies());
        }

        public async Task<IActionResult> Details(int? id)
        {
            return View(await _movieService.GetMovie(id.Value));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Constant.Movies_Controller_Bind_Attribute)] Movie movie)
        {
            await _movieService.CreateMovie(movie);

            return RedirectToAction(Constant.Movies_Controller_Action_Index);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _movieService.GetMovie(id.Value));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(Constant.Movies_Controller_Bind_Attribute)] Movie movie)
        {
            var response = await _movieService.UpdateMovie(id, movie);

            if (response)
            {
                return RedirectToAction(Constant.Movies_Controller_Action_Index);
            }
            else
            {
                Debug.WriteLine($"An error occurred");
                return View();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _movieService.GetMovie(id.Value));
        }
        
        [HttpPost, ActionName(Constant.Movies_Controller_Action_Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _movieService.DeleteMovie(id);

            if (response)
            {
                return RedirectToAction(Constant.Movies_Controller_Action_Index);
            }
            else
            {
                Debug.WriteLine($"An error occurred");
                return View();
            }
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Authorize(Roles = Constant.Role_Admin)]
        public async Task<IActionResult> OnlyAdmin()
        {
            var userInfo = await _movieService.GetUserInfo();
            
            return View(userInfo);
        }

        private async Task LogTokenAndClaims()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity token: {identityToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}
