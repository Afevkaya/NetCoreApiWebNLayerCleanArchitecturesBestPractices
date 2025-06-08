using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services;

public class FluentValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;
            if (validator is not null)
            {
                var validationContext = new ValidationContext<object>(argument);
                var result = await validator.ValidateAsync(validationContext);
                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                    var resultModel = ServiceResult.Fail(errors);
                    context.Result = new BadRequestObjectResult(resultModel);
                    return;
                }
            }
        }
        await next();
    }
}