using System.Net;

namespace BuberDinner.Application.Common.Errors;

public interface IError1
{
    public HttpStatusCode StatusCode { get; }
    public string Message { get; }
}