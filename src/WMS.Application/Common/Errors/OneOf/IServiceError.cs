
using System.Net;
namespace WMS.Application.Common.Errors.OneOf;

public interface IServiceError
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorMessage { get;  }
}
