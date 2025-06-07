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
    
    // Static Factory Methods
    public static ServiceResult<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK) => new ServiceResult<T> ()
    {
        Data = data, ErrorMessages = null, HttpStatusCode = statusCode
    };
    public static ServiceResult<T> Fail(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new ServiceResult<T>()
    {
        Data = default, ErrorMessages = errorMessages, HttpStatusCode = statusCode
    };
    public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new ServiceResult<T>()
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
    public static ServiceResult Success(HttpStatusCode statusCode = HttpStatusCode.OK) => new ServiceResult()
    {
        ErrorMessages = null, HttpStatusCode = statusCode
    };
    
    public static ServiceResult Fail(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new ServiceResult()
    {
        ErrorMessages = errorMessages, HttpStatusCode = statusCode
    };
    
    public static ServiceResult Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new ServiceResult()
    {
        ErrorMessages = [errorMessage], HttpStatusCode = statusCode
    };
}