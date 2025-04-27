using ErrorOr;
using FluentValidation;
using MediatR;

namespace FleetMan.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation(
                validationFailure.ErrorCode ?? validationFailure.PropertyName,
                validationFailure.ErrorMessage));

        // The compiler doesn't know about implicit conversion of List<Error> to ErrorOr<> so we have to cast it
        // It's fine to use it because we have a constraint on TResponse that it must be IErrorOr
        return (dynamic)errors;
    }
}