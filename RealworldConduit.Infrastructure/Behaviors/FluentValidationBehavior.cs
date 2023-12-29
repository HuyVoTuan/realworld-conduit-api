using FluentValidation;
using MediatR;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealworldConduit.Infrastructure.Filters
{
    public class FluentValidationBehavior<TRequest, TRespone> : IPipelineBehavior<TRequest, TRespone>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TRespone> Handle(TRequest request, RequestHandlerDelegate<TRespone> next, CancellationToken cancellationToken)
        {

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.SelectMany(result => result.Errors)
                                            .Where(f => f != null)
                                            .GroupBy(x => x.PropertyName)
                                            .ToList()
                                            .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage));

            if (failures.Any())
            {
                throw new RestException(HttpStatusCode.BadRequest, failures);
            }

            return await next();
        }
    }
}
