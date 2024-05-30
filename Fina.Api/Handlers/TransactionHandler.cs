using Fina.Api.Data;
using Fina.Core.Common;
using Fina.Core.Enums;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers
{
    public class TransactionHandler(AppDbContext context) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                Transaction transaction = new Transaction
                {
                    UserId = request.UserId,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.Now,
                    Amount = request.Amount,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    Title = request.Title,
                    Type = request.Type
                };

                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso!");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível criar a transação.");
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                Transaction? transaction = await context.Transactions
                                                  .FirstOrDefaultAsync(c => c.Id == request.Id &&
                                                                            c.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction?>(null, 404, "Transação não encontrada.");

                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, message: "Transação removida com sucesso!");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível remover a transação");
            }
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                Transaction? transaction = await context.Transactions
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(c => c.Id == request.Id &&
                                                                            c.UserId == request.UserId);

                return transaction is null
                    ? new Response<Transaction?>(null, 404, "Transação não encontrada.")
                    : new Response<Transaction?>(transaction);
            }
            catch (Exception)
            {
                return new Response<Transaction?>(null, 500, "Não foi possível recuperar esta transação");
            }
        }

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDay();
                request.EndDate ??= DateTime.Now.GetLastDay();
            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(null, message: "Não foi possível determinar a data de início e fim da transação.");
            }

            try
            {
                var query = context.Transactions
                                 .AsNoTracking()
                                 .Where(c => c.PaidOrReceivedAt>= request.StartDate &&
                                             c.PaidOrReceivedAt <= request.EndDate &&
                                             c.UserId == request.UserId)
                                 .OrderBy(c => c.PaidOrReceivedAt);

                var transactions = await query
                                     .Skip((request.PageNumber - 1) * request.PageSize)
                                     .Take(request.PageSize)
                                     .ToListAsync();

                int count = await query.CountAsync();

                return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(null, 0, request.PageNumber, request.PageSize);
            }
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                Transaction? transaction = await context.Transactions
                                                  .FirstOrDefaultAsync(c => c.Id == request.Id &&
                                                                            c.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction?>(null, 404, "Transação não encontrada.");

                transaction.CategoryId = request.CategoryId;
                transaction.Amount = request.Amount;
                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso!");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível atualizar a transação");
            }
        }
    }
}
