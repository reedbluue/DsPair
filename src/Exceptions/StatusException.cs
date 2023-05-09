using System.Net;

namespace DsPair.src.Exceptions;
public class StatusException: Exception {
  public HttpStatusCode statusCode;
  public StatusException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message) {
    this.statusCode = statusCode;
  }
}