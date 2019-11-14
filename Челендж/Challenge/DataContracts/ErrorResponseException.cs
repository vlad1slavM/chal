using Challenge.Extensions;
using System;
using System.Net;
using System.Net.Http;

namespace Challenge.DataContracts
{
    public class ErrorResponseException : Exception
    {
        public ErrorResponseException(HttpStatusCode statusCode, ErrorResponse errorResponse)
            : base(errorResponse.ErrorName)
        {
            StatusCode = statusCode;
            ErrorName = errorResponse.ErrorName;
            ErrorLevel = errorResponse.ErrorLevel;
            ErrorDescription = ErrorDescription;
        }

        public ErrorResponseException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
        public string ErrorName { get; }
        public string ErrorLevel { get; }
        public string ErrorDescription { get; }

        public static ErrorResponseException ExtractFrom(HttpResponseMessage response)
        {
            try
            {
                var errorResponse = response.Content.ReadAsAsync<ErrorResponse>().Result;
                return new ErrorResponseException(response.StatusCode, errorResponse);
            }
            catch (Exception)
            {
                return new ErrorResponseException(response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
