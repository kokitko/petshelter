using MediatR;
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace PetShelter.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(
    IValidator<TRequest>? validator = null,
    ILogger<TRequest>? logger = null) :
    IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (validator is null)
            return await next();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid)
            return await next();

        if (logger != null)
        {
            var errors = validationResult.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToList();
            
            logger.LogWarning(
                "Validation failed for {RequestName}: {Errors}",
                typeof(TRequest).Name,
                string.Join(" | ", errors));
        }

        var errorList = validationResult.Errors
            .Select(validationFailure => Error.Validation(
                validationFailure.PropertyName, 
                validationFailure.ErrorMessage))
            .ToList();
        return (dynamic)errorList;
    }
}
