using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// using StudentManagement.API.Models;

namespace MyApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Cho phép request tiếp tục đi vào các Controller xử lý bình thường
                await _next(context);
            }
            catch (Exception ex)
            {
                // Nếu bất kỳ chỗ nào phía trong bị crash, nó sẽ nhảy thẳng vào đây!
                _logger.LogError(ex, "Một lỗi hệ thống đã xảy ra: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            // 🔥 BƯỚC CẢI TIẾN: Tự động phân loại loại lỗi dựa trên kiểu Exception văng ra
            var statusCode = exception switch
            {
                // Nếu Service quăng lỗi KeyNotFoundException -> Trả về 404 Not Found
                KeyNotFoundException => HttpStatusCode.NotFound,

                // Nếu Service quăng lỗi InvalidOperationException -> Trả về 400 Bad Request
                InvalidOperationException => HttpStatusCode.BadRequest,

                // Các loại lỗi ArgumentException (Dữ liệu truyền vào sai) -> Trả về 400 Bad Request
                ArgumentException => HttpStatusCode.BadRequest,

                // Nếu là lỗi hệ thống khác (Lỗi DB, NullReference...) -> Trả về 500 Internal Server Error
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            // Tạo nội dung lỗi trả về cho client
            var response = _env.IsDevelopment()
                ? new ErrorDetails 
                  { 
                      StatusCode = context.Response.StatusCode, 
                      // Ở môi trường Dev, lỗi gì cũng show message thật ra để lập trình viên đọc sửa code
                      message = exception.Message, 
                      Details = exception.StackTrace?.ToString() 
                  }
                : new ErrorDetails 
                  { 
                      StatusCode = context.Response.StatusCode, 
                      // Ở môi trường Production, nếu lỗi 500 thì giấu chữ đi, còn lỗi 400/404 nghiệp vụ thì vẫn phải show chữ tiếng Việt ra cho người dùng biết lỗi gì để họ sửa dữ liệu
                      message = context.Response.StatusCode == (int)HttpStatusCode.InternalServerError 
                                ? "Đã xảy ra lỗi nghiêm trọng từ hệ thống. Vui lòng liên hệ Admin!" 
                                : exception.Message
                  };

            await context.Response.WriteAsync(response.ToString());
        }
    }
}