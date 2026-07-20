using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class AuthenticationService(IIdentityService identityService, ITokenService tokenService) : IAuthenticationService
    {
        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default)
            => await identityService.CheckEmailExistsAsync(email, ct);

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var userResult = await identityService.FindUserByIdAsync(email, ct);
            var user = userResult.data;
            var rolesResult = await identityService.GetUserRolesAsync(email, ct);
            var token = tokenService.CreateToken(user.Id, user.Email, user.DisplayName, rolesResult.data);

            return new UserDto() { DisplayName = user.DisplayName, Email = user.Email, Token = token };
        }

        public Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken ct = default)
            => identityService.GetUserAddressAsync(email, ct);


        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            var userResult = await identityService.FindUserByIdAsync(loginDto.Email, ct);

            if (!userResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors); 

            var passwordResult = await identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);
            if (!passwordResult.IsSuccess)
                return Result<UserDto>.Fail(passwordResult.Errors);

            if (!passwordResult.data)
                return Result<UserDto>.Fail(Error.Unauthorized("Invalid email or password"));

            var rolesResult = await identityService.GetUserRolesAsync(userResult.data.Email, ct);
            var token = tokenService.CreateToken(userResult.data.Id, userResult.data.Email, userResult.data.DisplayName, rolesResult.data);
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = loginDto.Email,
                DisplayName = userResult.data.DisplayName,
                Token = token
            });
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var userResult = await identityService.CreateUserAsync(registerDto, ct);
            if(!userResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);

            var rolesResult = await identityService.GetUserRolesAsync(userResult.data.Email, ct);
            var token = tokenService.CreateToken(userResult.data.Id, userResult.data.Email, userResult.data.DisplayName, rolesResult.data);
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = userResult.data.Email,
                DisplayName = userResult.data.DisplayName,
                Token = token
            });
        }

        public Task<Result<AddressDto>> UpsertUserAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default)
            => identityService.UpsertUserAddressAsync(email, addressDto, ct);

    }
}
