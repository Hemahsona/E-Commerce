using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Services
{
    internal class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
    {
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<bool>.Fail(Error.NotFound("User not found", $"User with email {email}"));
            else
                return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<Result<IdentityUserResult>> FindUserByIdAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IdentityUserResult>.Fail(Error.NotFound("User not found", $"User with email {email}"));

            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.DisplayName, user.UserName));
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errorrs = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errorrs);
            }

            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.DisplayName, user.UserName));
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound("User not found", $"User with email {email} is not found"));
            var roles = await userManager.GetRolesAsync(user);
            return Result<IReadOnlyList<string>>.Ok(roles.ToList());
        }

        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(a => a.Address).FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user?.Address == null)
                return Result<AddressDto>.Fail(Error.NotFound("Address not found", $"Address for user with email {email} is not found"));

            return Result<AddressDto>.Ok(new AddressDto
            {
                City = user.Address.City,
                Street = user.Address.Street,
                Country = user.Address.Country,
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName
            });
        }

        public async Task<Result<AddressDto>> UpsertUserAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(a => a.Address).FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user?.Address == null)
            {
                user.Address = new Address
                {
                    City = addressDto.City,
                    Street = addressDto.Street,
                    Country = addressDto.Country,
                    FirstName = addressDto.FirstName,
                    LastName = addressDto.LastName
                };

            }
            else
            {
                user.Address.City = addressDto.City;
                user.Address.Street = addressDto.Street;
                user.Address.Country = addressDto.Country;
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;

            }
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<AddressDto>.Fail(errors);
            }
            return Result<AddressDto>.Ok(addressDto);
        }
    }
}
