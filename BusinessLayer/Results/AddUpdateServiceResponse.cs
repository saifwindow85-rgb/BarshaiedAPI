using BusinessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Results
{
    public  class AddUpdateServiceResponse<T>
    {
        public T? Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public List<string>?Errors { get; private set; }
        public EnErrorTypes ?ErrorType { get; private set; }

        public static AddUpdateServiceResponse<T> Success(T data) => new AddUpdateServiceResponse<T>
        {
            Data = data,
            IsSuccess = true
        };

        public static AddUpdateServiceResponse<T> Failure(List<string>errors,EnErrorTypes errorType) => new AddUpdateServiceResponse<T>
        {
            Errors = errors,
            ErrorType = errorType,
            IsSuccess = false
        };

        public static AddUpdateServiceResponse<T> InvalidRelatedData() => new AddUpdateServiceResponse<T>
        {
            ErrorType = EnErrorTypes.InvalidData,
            Errors = new List<string>() { "The operation failed due to invalid related data. One or more referenced records do not exist" }
        };
    }
}
