using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController(IAuthenticationService authenticationService) : ApiBaseController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct)
            => TOActionResult(await authenticationService.LoginAsync(loginDto, ct));

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct)
            => TOActionResult(await authenticationService.RegisterAsync(registerDto, ct));

        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email, CancellationToken ct)
            => TOActionResult(await authenticationService.CheckEmailExistsAsync(email, ct));

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
          => TOActionResult(await authenticationService.GetCurrentUserAsync(GetCurrentUserEmail(), ct));




        [Authorize]
        [HttpGet("userAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress(CancellationToken ct)
           => TOActionResult(await authenticationService.GetUserAddressAsync(GetCurrentUserEmail(), ct));

        [Authorize]
        [HttpPut("userAddress")]
        public async Task<ActionResult<AddressDto>> UpsertUserAddress(AddressDto addressDto, CancellationToken ct)
            => TOActionResult(await authenticationService.UpsertUserAddressAsync(GetCurrentUserEmail(), addressDto, ct));


    }
}
