﻿using System.Net;

namespace StockFlow.Application.Helpers;

public class CustomException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public CustomException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public static class ErrorHelper
{
    public static void ThrowNotFoundException(string message = "Not found!")
    {
        throw new CustomException(message, HttpStatusCode.NotFound);
    }

    public static void ThrowBadRequestException(string message = "Invalid request!")
    {
        throw new CustomException(message, HttpStatusCode.BadRequest);
    }

    public static void ThrowConflictException(string message = "Conflict detected!")
    {
        throw new CustomException(message, HttpStatusCode.Conflict);
    }
}
