using System.Net;
using Application.DTOs;
using Application.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    [Route("[controller]")]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        public IActionResult Error(HttpStatusCode code)
        {
            var error = new ApiResponse(code);
            var errorReturn = new ApiResponseDto
            {
              ErrorMessage  = error.ErrorMessage,
              StatusCode = error.StatusCode
            };
            return new ObjectResult(errorReturn);
        }
    }
}