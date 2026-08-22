using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers
{

    public class PaymentsController(IPaymentService paymentService, IOptions<PaymentGatewaySetting> paymentGatewaySettings) : ApiBaseController
    {
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdate(string basketId, CancellationToken ct)
            => TOActionResult(await paymentService.CreateOrUpdateAsync(basketId, ct));
        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] object payload)
        {
            var requestJson = new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = Stripe.EventUtility.ConstructEvent(await requestJson,
                    Request.Headers["Stripe-Signature"], paymentGatewaySettings.Value.WebhookSecret);

                switch(stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        if(paymentIntent != null)
                        {
                            await paymentService.PaymentSucceededAsync(paymentIntent.Id);
                        }
                        break;
                    case EventTypes.PaymentIntentPaymentFailed:
                        var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        if(failedPaymentIntent != null)
                        {
                            await paymentService.PaymentFailedAsync(failedPaymentIntent.Id);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch(StripeException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }



            // Handle webhook logic here
            return Ok();
        }
    }
}
