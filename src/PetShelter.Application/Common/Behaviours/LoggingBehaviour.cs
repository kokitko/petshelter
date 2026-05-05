using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PetShelter.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger) : 
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid().ToString("N").Substring(0, 8);
        
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["RequestId"] = requestId,
            ["RequestName"] = requestName,
            ["Timestamp"] = DateTime.UtcNow
        }))
        {
            logger.LogInformation(
                "Starting request: {RequestName} [RequestId: {RequestId}]",
                requestName,
                requestId);

            var sw = Stopwatch.StartNew();
            
            try
            {
                var response = await next();
                
                sw.Stop();
                logger.LogInformation(
                    "Completed request: {RequestName} [RequestId: {RequestId}] | Execution time: {ElapsedMs}ms",
                    requestName,
                    requestId,
                    sw.ElapsedMilliseconds);
                
                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();
                logger.LogError(
                    ex,
                    "Error in request: {RequestName} [RequestId: {RequestId}] | Execution time: {ElapsedMs}ms",
                    requestName,
                    requestId,
                    sw.ElapsedMilliseconds);
                
                throw;
            }
        }
    }
}
