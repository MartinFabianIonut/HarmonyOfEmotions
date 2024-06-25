﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HarmonyOfEmotions.Domain.Exceptions
{
	public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
	{
		private readonly ILogger<GlobalExceptionFilter> _logger = logger;

		public void OnException(ExceptionContext context)
		{
			_logger.LogError(context.Exception, "An unhandled exception occurred.");

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
