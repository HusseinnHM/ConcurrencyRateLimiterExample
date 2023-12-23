

# ConcurrencyRateLimiterExample

*Scenario*: We have a flight booking system with high read data requests and an endpoint for booking where concurrency requests may occur.

*Problem*: Handling two users trying to book the same seat on the same flight.

*Challenge 1*: Locking database records (Pessimistic Concurrency) will affect reading data as well.<br>
*Challenge 2*: Row Versioning (Optimistic Concurrency) works, but if two users try to book different seats for the same flight, one of them will fail.

***Solution***: Using built-in Concurrency Rate Limiting, we can build a RateLimiterPolicy with seat number and flight id as key. So, only if two users try to book the same seat on the same flight, the policy will handle it.

