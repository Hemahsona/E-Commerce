using E_Commerce.Application.Contract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Services
{
    public class UrlService(IHttpContextAccessor httpContextAccessor) : IUrlService
    {
        public string GetImageUrl(string imageName)
        {
            var request = httpContextAccessor.HttpContext!.Request;

            return $"{request.Scheme}://{request.Host}/Files/{imageName}";
        }
    }
}
