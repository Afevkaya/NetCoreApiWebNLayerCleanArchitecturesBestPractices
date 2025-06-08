using System.Net;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class CustomController: ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult<T>(ServiceResult<T> result)
    {
        return result.HttpStatusCode switch
        {
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.Created => Created(result.UrlAsCreated, result.Data),
            _ => new ObjectResult(result) { StatusCode = result.HttpStatusCode.GetHashCode() }
        };
    }
    
    [NonAction]
    public IActionResult CreateActionResult(ServiceResult result)
    {
        return result.HttpStatusCode switch
        {
            HttpStatusCode.NoContent => NoContent(),
            _ => new ObjectResult(result) { StatusCode = result.HttpStatusCode.GetHashCode() }
        };
    }
}