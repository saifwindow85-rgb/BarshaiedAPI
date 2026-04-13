using BusinessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Results
{
    public  class CategoryServiceResponse<T>
    {
        public T? Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public List<string>?Errors { get; private set; }
        public EnErrorTypes ?ErrorType { get; private set; }

        public static CategoryServiceResponse<T> Success(T data) => new CategoryServiceResponse<T>
        {
            Data = data,
            IsSuccess = true
        };

        public static CategoryServiceResponse<T> Failure(List<string>errors,EnErrorTypes errorType) => new CategoryServiceResponse<T>
        {
            Errors = errors,
            ErrorType = errorType,
            IsSuccess = true
        };
    }
}
