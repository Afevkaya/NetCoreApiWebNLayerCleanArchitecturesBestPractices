using System.Net;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services.Filters;

public class NotFoundFilter<T,TId>(IGenericRepository<T,TId> genericRepository): Attribute, IAsyncActionFilter where T 
    : class where TId : struct
{
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        #region First Code Snippet

        // var idValue = context.ActionArguments.Values.FirstOrDefault();
        // var idKey = context.ActionArguments.Keys.FirstOrDefault();
        // if(idValue == null && idKey != "id")
        // {
        //     await next();
        //     return;
        // }
        //
        // if (idValue is not TId id)
        // {
        //     await next();
        //     return;
        // }
        //
        // if (!await genericRepository.AnyAsync(id))
        // {
        //     var entityName = typeof(T).Name;
        //     var actionName = context.ActionDescriptor.RouteValues["action"];
        //     var result = ServiceResult.Fail(
        //         $"{entityName} ({id}) bulunamadı. Lütfen {actionName} işlemini kontrol edin.",
        //         HttpStatusCode.NotFound
        //     );
        //     context.Result = new NotFoundObjectResult(result);
        //     return;
        // }
        // await next();

        #endregion
        
        if (!context.ActionArguments.TryGetValue("id", out var idObj) || idObj is not TId id)
        {
            await next();
            return;
        }

        if (!await genericRepository.AnyAsync(id))
        {
            var entityName = typeof(T).Name;
            var actionName = context.ActionDescriptor.RouteValues["action"];
            var result = ServiceResult.Fail(
                $"{entityName} ({id}) bulunamadı. Lütfen {actionName} işlemini kontrol edin.",
                HttpStatusCode.NotFound
            );
            context.Result = new NotFoundObjectResult(result);
            return;
        }
        await next();
    }
}