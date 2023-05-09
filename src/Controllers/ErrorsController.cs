using DsPair.src.Exceptions;
using DsPair.src.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DsPair.src.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController: ControllerBase {
  [Route("error")]
  public ErrorMessage Error() {
    var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
    var exception = context.Error;

    Response.StatusCode = (int) System.Net.HttpStatusCode.InternalServerError;

    if(exception is StatusException) {
      Response.StatusCode = (int) ((StatusException) exception).statusCode;
    }
    return new ErrorMessage(exception.Message, exception.GetType().Name);
  }
}
