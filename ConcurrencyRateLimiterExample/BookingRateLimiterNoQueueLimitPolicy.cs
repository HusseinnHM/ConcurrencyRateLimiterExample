using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ConcurrencyRateLimiterExample;

public class BookingRateLimiterNoQueueLimitPolicy : IRateLimiterPolicy<string>
{
    public const string PolicyName = nameof(BookingRateLimiterNoQueueLimitPolicy);

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        var key = $"{httpContext.Request.Query["flyId"]}-{httpContext.Request.Query["seatNumber"]}";
        return RateLimitPartition.GetConcurrencyLimiter(key, _ => new ConcurrencyLimiterOptions
        {
            PermitLimit = 1,
            QueueLimit = 0,
        });
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.HttpContext.Response.WriteAsJsonAsync("Someone tries book same seat :)", cancellationToken: token);
    };
}