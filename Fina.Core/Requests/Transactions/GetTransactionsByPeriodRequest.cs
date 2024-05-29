namespace Fina.Core.Requests.Transactions
{
    public class GetTransactionsByPeriodRequest : PagedResponse
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
