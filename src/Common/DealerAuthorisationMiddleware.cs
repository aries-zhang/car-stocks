using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using CarStocks.Entities;
using CarStocks.Repositories;

namespace CarStocks.Common
{
    public class DealerAuthorisationMiddleware
    {
        private const string AUTH_HEADER_KEY = "Authorization";
        private readonly RequestDelegate _next;

        public DealerAuthorisationMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, IDealerRepository dealerRepository)
        {
            // Authorization: Token h480djs93hd8
            var dealer = ValidateDealerToken(context, dealerRepository);
            if (dealer != null)
            {
                context.Items[Constants.DEALER_ID_CONTEXT_KEY] = dealer.Id;
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }

        private Dealer ValidateDealerToken(HttpContext context, IDealerRepository dealerRepository)
        {
            if (!context.Request.Headers.Any(h => h.Key == AUTH_HEADER_KEY))
            {
                return null;
            }

            var authHeader = context.Request.Headers[AUTH_HEADER_KEY].ToString();

            if (!authHeader.StartsWith("Token "))
            {
                return null;
            }

            var dealerToken = authHeader.Split(" ")[1];

            if (string.IsNullOrEmpty(dealerToken))
            {
                return null;
            }

            return dealerRepository.GetByToken(dealerToken);
        }
    }


    public static class DealerAuthorisationMiddlewareExtension
    {
        public static IApplicationBuilder UseDealerAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DealerAuthorisationMiddleware>();
        }
    }
}
