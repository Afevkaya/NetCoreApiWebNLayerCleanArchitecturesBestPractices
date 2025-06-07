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
        if (result.HttpStatusCode == HttpStatusCode.NoContent)
            return new ObjectResult(null) { StatusCode = result.HttpStatusCode.GetHashCode() };
        return new ObjectResult(result) {StatusCode = result.HttpStatusCode.GetHashCode()};
    }
    
    [NonAction]
    public IActionResult CreateActionResult(ServiceResult result)
    {
        if (result.HttpStatusCode == HttpStatusCode.NoContent)
            return new ObjectResult(null) { StatusCode = result.HttpStatusCode.GetHashCode() };
        return new ObjectResult(result) {StatusCode = result.HttpStatusCode.GetHashCode()};
    }
}