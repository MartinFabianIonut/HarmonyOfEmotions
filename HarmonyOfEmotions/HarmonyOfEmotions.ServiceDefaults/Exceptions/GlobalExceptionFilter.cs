using HarmonyOfEmotions.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HarmonyOfEmotions.ServiceDefaults.Exceptions
{
	public class GlobalExceptionFilter() : IExceptionFilter
	{

		public void OnException(ExceptionContext context)
		{
			var statusCode = context.Exception switch
			{
				ExternalServiceException => 503,
				InternalServerErrorException => 500,
				_ => 500
			};

			var error = new ErrorResponse { StatusCode = statusCode, Message = context.Exception.Message };

			context.Result = new JsonResult(error) { StatusCode = statusCode };
			context.ExceptionHandled = true;
		}
	}
}
