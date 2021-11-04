using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Errors;
using Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            var response = new ApiException(0, "");
            int statusCode = 0;

            switch (ex)
            {
                case ApiException apiEx:
                    statusCode = (int) apiEx.StatusCode;
                    if (_env.IsDevelopment())
                    {
                        apiEx.Details = apiEx.StackTrace;
                        response = apiEx;
                    }
                    else
                    {
                        response = apiEx;
                    }
                    break;
                
                case { } e: 
                    statusCode = (int) HttpStatusCode.InternalServerError;
                    response = _env.IsDevelopment()
                        ? new ApiException((HttpStatusCode) statusCode, e.Message, e.StackTrace)
                        : new ApiException(HttpStatusCode.InternalServerError, e.Message);
                    break;
            }
            
            context.Response.StatusCode = statusCode;
            var responseReturn = _env.IsDevelopment()
                ? new ApiExceptionDto{StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage, 
                    Details = response.Details}
                : new ApiExceptionDto{StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };
            
            FileErrorHelper.SaveError(_configuration, statusCode, response.ErrorMessage, response.Details);
            
            var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            var json = JsonSerializer.Serialize(responseReturn, options);

            await context.Response.WriteAsync(json);
        }
    }
}