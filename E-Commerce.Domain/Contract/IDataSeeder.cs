using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contract
{
    public interface IDataSeeder
    {
        Task SeedDataAsync(CancellationToken ct= default);
    }
}
