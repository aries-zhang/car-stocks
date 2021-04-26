using CarStocks.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStocks.Common
{
    public class DealerAuthorisationMiddleware
    {
        private readonly RequestDelegate _next;
        private IDealerRepository _dealerRepository;

        public DealerAuthorisationMiddleware(RequestDelegate next, IDealerRepository dealerRepository)
        {
            this._next = next;
            this._dealerRepository = dealerRepository;
        }

        public async Task Invoke(HttpContext context)
        {
            // Authorization: Token token=h480djs93hd8
            string authHeader = context.Request.Headers["Authorization"];

            // TODO: header validation

            // string token = authHeader.Split("=")[1].Trim();

            // TODO: data validation
            // context.Items[Constants.DEALER_ID_CONTEXT_KEY] = this._dealerRepository.GetByToken(token).Id;
        }
    }
}
