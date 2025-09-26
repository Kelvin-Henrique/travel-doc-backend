using System;

namespace TravelDoc.Infrastructure.Core.Results
{
    public struct Result : IResult
    {
        public bool IsSuccess { get; private set; }

        public string Error { get; private set; }

        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
        {
            return new Result(isSuccess: true, string.Empty);
        }

        public static Result Failure(string error)
        {
            return new Result(isSuccess: false, error);
        }

        public static Result Failure(string message, Exception ex)
        {
            return Failure(string.Format("{0}. Exception: {1} Message: {2} Inner Exception: {3} Stack: {4}", message, "Exception", ex.Message, ex.InnerException, ex.StackTrace));
        }
    }
}
