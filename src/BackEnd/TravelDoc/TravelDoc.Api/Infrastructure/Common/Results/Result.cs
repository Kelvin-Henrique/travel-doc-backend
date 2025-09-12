using System;

namespace TravelDoc.Infrastructure.Core.Results
{
    public struct Result : IResult
    {
        private Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public bool IsFailure => !IsSuccess;

        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result Success(string message)
        {
            return new Result(true, message);
        }

        public static Result Failure(string error)
        {
            return new Result(false, error);
        }

        public static Result Failure(string message, Exception ex)
        {
            return Failure($"{message}. Exception: {nameof(Exception)} Message: {ex.Message} Inner Exception: {ex.InnerException} Stack: {ex.StackTrace}");
        }
    }
}
