using System.Reflection.Metadata.Ecma335;

namespace E_Commerce.Application.Common
{
    public class PaymentGatewaySetting
    {
        public string SecretKey { get; set; } = default!;
        public string DefaultCurrency { get; set; } = default!;
        public string WebhookSecret { get; set; } = default!;
    }
}
