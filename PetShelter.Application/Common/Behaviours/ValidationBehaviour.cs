using MediatR;
using ErrorOr;
using FluentValidation;

namespace PetShelter.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(
    IValidator<TRequest>? validator = null) :
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

        var errors = validationResult.Errors
            .Select(validationFailure => Error.Validation(
                validationFailure.PropertyName, 
                validationFailure.ErrorMessage))
            .ToList();
        return (dynamic)errors;
    }
}
