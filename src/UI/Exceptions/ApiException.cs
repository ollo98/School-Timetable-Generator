﻿using Application.Responses;
using System;
using System.Net;

namespace UI.Exceptions
{
    public class ApiException : Exception
    {
        public ErrorResult ErrorResult { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string[] Errors { get; set; }

        public ApiException(ErrorResult error, HttpStatusCode statusCode, string[] errors)
        {
            ErrorResult = error;
            StatusCode = statusCode;
            Errors = errors;
        }

        public ApiException(ErrorResult error, HttpStatusCode statusCode)
        {
            ErrorResult = error;
            StatusCode = statusCode;
        }

        public ApiException(ErrorResult error)
        {
            ErrorResult = error;
        }
    }
}
