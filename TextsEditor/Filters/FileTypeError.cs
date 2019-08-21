using System;
using System.Web;
using System.Web.Mvc;

namespace TextsEditor.Filters
{
    public class FileTypeError : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is HttpException)
            {
                exceptionContext.Result = new RedirectResult("/Home/ErrorType");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }
}