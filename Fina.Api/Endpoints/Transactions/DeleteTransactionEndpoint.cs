using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using System.Security.Claims;

namespace Fina.Api.Endpoints.Transactions
{
    public class DeleteTransactionEndpoint :IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Remove uma transação")
            .WithDescription("Remove uma transação")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, ITransactionHandler handler, long id)
        {
            DeleteTransactionRequest? request = new DeleteTransactionRequest
            {
                UserId = ApiConfiguration.UserId,
                Id = id
            };

            var result = await handler.DeleteAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
