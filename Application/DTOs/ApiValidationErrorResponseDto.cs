using System.Collections.Generic;

namespace Application.DTOs
{
    public class ApiValidationErrorResponseDto : ApiResponseDto
    {
        public IEnumerable<string> Errors { get; set; }    
    }
}