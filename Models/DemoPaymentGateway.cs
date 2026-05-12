namespace SmartPark.Services
{
    public class DemoPaymentResult
    {
        public bool Success { get; init; }
        public string? TransactionId { get; init; }
        public string? FailureReason { get; init; }
    }

    public interface IPaymentGateway
    {
        Task<DemoPaymentResult> ChargeAsync(decimal amount, string method, string userId);
    }

    public class DemoPaymentGateway : IPaymentGateway
    {
        private readonly Random _rng = new();

        public Task<DemoPaymentResult> ChargeAsync(decimal amount, string method, string userId)
        {
            // 80% success demo
            var ok = _rng.Next(1, 101) <= 80;

            if (ok)
            {
                return Task.FromResult(new DemoPaymentResult
                {
                    Success = true,
                    TransactionId = $"DEMO-{Guid.NewGuid():N}"
                });
            }

            return Task.FromResult(new DemoPaymentResult
            {
                Success = false,
                FailureReason = "Demo payment failed (simulated). Please try again."
            });
        }
    }
}