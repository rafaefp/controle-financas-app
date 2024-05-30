﻿using Fina.Api.Common.Api;
using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions
{
    public class GetTransactionsByPeriodEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get By Period")
            .WithSummary("Recupera as transações por período")
            .WithDescription("Recupera as transações por período")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>?>>();

        private static async Task<IResult> HandleAsync(ITransactionHandler handler,
                                                       [FromQuery] DateTime? startDate = null,
                                                       [FromQuery] DateTime? endDate = null,
                                                       [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
                                                       [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            GetTransactionsByPeriodRequest? request = new GetTransactionsByPeriodRequest
            {
                UserId = ApiConfiguration.UserId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await handler.GetByPeriodAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
