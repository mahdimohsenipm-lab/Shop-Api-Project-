using Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebFramework.Api
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        
        public string Message { get; set; }

        public ApiResultStatusCode StatusCode { get; set; }


        public ApiResult(bool issucces, ApiResultStatusCode apiResultStatusCode, string message = null)
        {
            IsSuccess = issucces;
            Message = message ?? apiResultStatusCode.ToDisplay();
            StatusCode = apiResultStatusCode;

        }



        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.Notfound);
        }
    }
    #endregion

    public class ApiResult<T> : ApiResult
        where T : class
    {
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }

        public ApiResult(bool issucces, ApiResultStatusCode apiResultStatusCode, T data, string message = null)
            : base(issucces, apiResultStatusCode, message)
        {

            Data = data;
        }


        #region Implicit Operators
        public static implicit operator ApiResult<T>(T data)
        {
            return new ApiResult<T>(true, ApiResultStatusCode.Success, data);
        }

        public static implicit operator ApiResult<T>(OkResult result)
        {
            return new ApiResult<T>(true, ApiResultStatusCode.Success, null);
        }

        public static implicit operator ApiResult<T>(OkObjectResult result)
        {
            return new ApiResult<T>(true, ApiResultStatusCode.Success, (T)result.Value);
        }

        public static implicit operator ApiResult<T>(BadRequestResult result)
        {
            return new ApiResult<T>(false, ApiResultStatusCode.BadRequest, null);
        }

        public static implicit operator ApiResult<T>(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult<T>(false, ApiResultStatusCode.BadRequest, null, message);
        }

        public static implicit operator ApiResult<T>(ContentResult result)
        {
            return new ApiResult<T>(true, ApiResultStatusCode.Success, null, result.Content);
        }

        public static implicit operator ApiResult<T>(NotFoundResult result)
        {
            return new ApiResult<T>(false, ApiResultStatusCode.Notfound, null);
        }

        public static implicit operator ApiResult<T>(NotFoundObjectResult result)
        {
            return new ApiResult<T>(false, ApiResultStatusCode.Notfound, (T)result.Value);
        }
        #endregion


    }
}
