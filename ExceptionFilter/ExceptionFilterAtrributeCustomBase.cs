using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace _2_Exception_TrinhCV.ExceptionFilter
{
    public class ExceptionFilterAtrributeCustomBase : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<HttpActionExecutedContext>> _exceptionHandlers;
        public ExceptionFilterAtrributeCustomBase()
        {
            this._exceptionHandlers = new Dictionary<Type, Action<HttpActionExecutedContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(ArgumentNullException), HandleValidationException },
                { typeof(HttpRequestException), HandleHttpRequestException },
            };
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception == null) return;
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }
            HandleUnknownException(context);
        }

        private void HandleValidationException(HttpActionExecutedContext context)
        {
            HandleException(context, HttpStatusCode.BadRequest);
        }

        private void HandleHttpRequestException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as HttpRequestException;
            HandleException(context, exception.StatusCode);
        }

        private void HandleUnknownException(HttpActionExecutedContext context)
        {
            HandleException(context, HttpStatusCode.InternalServerError);
        }

        private void HandleException(HttpActionExecutedContext context, HttpStatusCode? httpStatusCode)
        {
            context.Response = new HttpResponseMessage((HttpStatusCode)httpStatusCode)
            {
                ReasonPhrase = context.Exception.Message,
                Content = new StringContent(JsonConvert.SerializeObject(new HttpResponseModel
                {
                    Error = new HttpErrorModel { Code = httpStatusCode.ToString(), Message = context.Exception.Message, Errors = null },
                    Data = null
                }))
            };

            context.Exception = null;
        }
    }
}