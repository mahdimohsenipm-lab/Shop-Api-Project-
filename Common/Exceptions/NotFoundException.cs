using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFramework;
using WebFramework.Api;

namespace Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException()
           : base(ApiResultStatusCode.Notfound)
        {
        }

        public NotFoundException(string message)
            : base(ApiResultStatusCode.Notfound, message)
        {
        }

        public NotFoundException(object additionalData)
            : base(ApiResultStatusCode.Notfound, additionalData)
        {
        }

        public NotFoundException(string message, object additionalData)
            : base(ApiResultStatusCode.Notfound, message, additionalData)
        {
        }

        public NotFoundException(string message, Exception exception)
            : base(ApiResultStatusCode.Notfound, message, exception)
        {
        }

        public NotFoundException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCode.Notfound, message, exception, additionalData)
        {
        }
    }
}
