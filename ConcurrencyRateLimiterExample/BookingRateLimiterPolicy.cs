using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ConcurrencyRateLimiterExample;

public class BookingRateLimiterPolicy : IRateLimiterPolicy<string>
{
    public const string PolicyName = nameof(BookingRateLimiterPolicy);

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        var key = $"{httpContext.Request.Query["flyId"]}-{httpContext.Request.Query["seatNumber"]}";
        return RateLimitPartition.GetConcurrencyLimiter(key, _ => new ConcurrencyLimiterOptions
        {
            PermitLimit = 1,
            QueueLimit = int.MaxValue,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.HttpContext.Response.WriteAsJsonAsync("Someone tries book same seat :)", cancellationToken: token);
    };
}