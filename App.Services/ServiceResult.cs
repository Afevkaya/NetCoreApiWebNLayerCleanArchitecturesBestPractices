using System.Net;
using System.Text.Json.Serialization;

namespace App.Services;

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public List<string>? ErrorMessages { get; set; }
    [JsonIgnore] public bool IsSuccess => ErrorMessages == null || ErrorMessages.Count == 0;
    [JsonIgnore] public bool IsFail => !IsSuccess;
    [JsonIgnore] public HttpStatusCode HttpStatusCode { get; set; }
    [JsonIgnore] public string? UrlAsCreated { get; set; }
    
    // Static Factory Methods
    public static ServiceResult<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK) => new()
    {
        Data = data, ErrorMessages = null, HttpStatusCode = statusCode
    };
    public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated) => new ()
    {
        Data = data, ErrorMessages = null, HttpStatusCode = HttpStatusCode.Created, UrlAsCreated = urlAsCreated
    };
    public static ServiceResult<T> Fail(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        Data = default, ErrorMessages = errorMessages, HttpStatusCode = statusCode
    };
    public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new ()
    {
        Data = default, ErrorMessages = [errorMessage], HttpStatusCode = statusCode
    };
}

public class ServiceResult
{
    public List<string>? ErrorMessages { get; set; }
    [JsonIgnore] public bool IsSuccess => ErrorMessages == null || ErrorMessages.Count == 0;
    [JsonIgnore] public bool IsFail => !IsSuccess;
    [JsonIgnore] public HttpStatusCode HttpStatusCode { get; set; }

    // Static Factory Methods
    public static ServiceResult Success() => new()
    {
        ErrorMessages = null, HttpStatusCode = HttpStatusCode.NoContent
    };
    
    public static ServiceResult Fail(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        ErrorMessages = errorMessages, HttpStatusCode = statusCode
    };
    
    public static ServiceResult Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        ErrorMessages = [errorMessage], HttpStatusCode = statusCode
    };
}