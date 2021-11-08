using Blogifier.Services;
using Blogifier.Shared.ViewModels.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService account)
        {
            _accountService = account;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn(SignInRequest request)
        {
            return Ok(_accountService.SignIn(request));
        }

        [HttpPost]
        [Authorize]
        [Route("signout")]
        public IActionResult SignOut(string email)
        {
            _accountService.SignOut(email); return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("refresh")]
        public IActionResult RefreshToken(RefreshRequest request)
        {
            return Ok(_accountService.RefreshToken(request));
        }
    }
}
