using System;

namespace TravelDoc.Infrastructure.Core.Results
{
    public struct Result<T> : IResult, IValue<T>
    {
        private Result(bool isSuccess, string error, T value)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }

        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public T Value { get; private set; }
        public bool IsFailure => !IsSuccess;

        public static Result<T> Success()
        {
            return new Result<T>(true, string.Empty, default);
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, string.Empty, data);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, error, default);
        }

        public static Result<T> Failure(string message, Exception ex)
        {
            return Failure($"{message}. Exception: {nameof(Exception)} Message: {ex.Message} Inner Exception: {ex.InnerException} Stack: {ex.StackTrace}");
        }

        public static implicit operator Result(Result<T> result)
        {
            return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
        }

        public static implicit operator Result<T>(Result result)
        {
            return result.IsSuccess ? Success() : Failure(result.Error);
        }
    }
}
