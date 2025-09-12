using System;

namespace TravelDoc.Infrastructure.Core.Results
{
    public struct Result<T, E> : IResult<T, E>
    {
        private Result(bool isSuccess, string error, T value, E errorValue)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
            ErrorValue = errorValue;
        }

        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public T Value { get; private set; }
        public bool IsFailure => !IsSuccess;
        public E ErrorValue { get; private set; }

        public static Result<T, E> Success()
        {
            return new Result<T, E>(true, string.Empty, default, default);
        }

        public static Result<T, E> Success(T data)
        {
            return new Result<T, E>(true, string.Empty, data, default);
        }

        public static Result<T, E> Failure(string error)
        {
            return new Result<T, E>(false, error, default, default);
        }

        public static Result<T, E> Failure(E error)
        {
            return new Result<T, E>(false, string.Empty, default, error);
        }

        public static Result<T, E> Failure(string message, Exception ex)
        {
            return Failure($"{message}. Exception: {nameof(Exception)} Message: {ex.Message} Inner Exception: {ex.InnerException} Stack: {ex.StackTrace}");
        }

        public static Result<T, E> Failure(string message, E item)
        {
            return new Result<T, E>(false, message, default, item);
        }

        public static implicit operator Result(Result<T, E> result)
        {
            return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
        }

        public static implicit operator Result<T, E>(Result result)
        {
            return result.IsSuccess ? Success() : Failure(result.Error);
        }
    }
}
