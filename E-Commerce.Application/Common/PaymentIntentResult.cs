using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public sealed class PaymentIntentResult
    {
        public string PaymentIntentId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}
