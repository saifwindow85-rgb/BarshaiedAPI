using BusinessLayer.Enums;
using BusinessLayer.Results;
using Microsoft.AspNetCore.Mvc;

namespace BarshaiedAPI.Extensions
{
    public static class ServicesResultExtension
    {
        public static ActionResult<AddUpdateServiceResponse<T>> ToActionResult<T>(this AddUpdateServiceResponse<T> response)
        {
            if (response.IsSuccess)
            {
                return response.Data != null ? new OkObjectResult(response.Data) : new NoContentResult();
            }

            return response.ErrorType switch
            {
                EnErrorTypes.NotFound => new NotFoundObjectResult(new
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = response.Errors.FirstOrDefault() ?? "The requested resource was not found."
                }),

                EnErrorTypes.InvalidData => new BadRequestObjectResult(new
                {
                    Status = 400,
                    Title = "Validation Error",
                    Errors = response.Errors // قائمة الأخطاء من الـ Validator
                }),

                EnErrorTypes.InvalidRefrenceData => new BadRequestObjectResult(new
                {
                    Status = 400,
                    Title = "Invalid Related Entity",
                    Errors = response.Errors
                }),

                _ => new ObjectResult(new { Status = 500, Title = "Server Error" }) { StatusCode = 500 }
            };
        }
    }
}
