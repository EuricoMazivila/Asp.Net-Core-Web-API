using System.Net;
namespace Application.DTOs
{
    public class ApiResponseDto
    {
         public HttpStatusCode StatusCode { get; set; }
         public string ErrorMessage { get; set; }
    }
}