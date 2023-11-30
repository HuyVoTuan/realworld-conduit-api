using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RealworldConduit.Infrastructure.Filters
{
    public class EntitiesAttributeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Select(e => new
                {
                    ErrorMessages = e.Value?.Errors.Select(x => x.ErrorMessage),
                    e.Key
                })
                .ToDictionary(e => e.Key, e => e.ErrorMessages?.Select(em => em));

                context.Result = new BadRequestObjectResult(errors);
            }
        }
    }
}
