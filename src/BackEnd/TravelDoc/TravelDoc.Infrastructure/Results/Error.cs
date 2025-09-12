using System;
using System.Collections.Generic;
using System.Text;

namespace TravelDoc.Infrastructure.Core.Results
{
    public sealed class Error : ValueObject<Error>
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        protected override bool EqualsCore(Error other)
        {
            return Code == other.Code && Message == other.Message;
        }

        protected override int GetHashCodeCore()
        {
            return HashCode.Combine(Code, Message);
        }

        public static implicit operator KeyValuePair<string, string>(Error error)
        {
            return new KeyValuePair<string, string>(error.Code, error.Message);
        }
    }

    public static class Errors
    {
        public static class General
        {
            public static Error NotFound(long? id = null)
            {
                string forId = id == null ? "" : $" for Id '{id}'";
                return new Error("record.not.found", $"Record not found{forId}");
            }

            public static Error NotFound(string message)
            {
                return new Error("record.not.found", message);
            }

            public static Error Duplicated(string id = null, string description = null)
            {
                if (!string.IsNullOrEmpty(description))
                {
                    return new Error("record.duplicated", description);
                }

                string forId = id == null ? "" : $" for Id '{id}'";
                return new Error("record.duplicated", $"Record already exists{forId}");
            }

            public static Error BadRequest(string description)
            {
                if (!string.IsNullOrEmpty(description))
                {
                    return new Error("invalid.request", description);
                }

                return new Error("invalid.request", "Invalid request");
            }

            public static Error ValueIsInvalid() =>
                new Error("value.is.invalid", "Value is invalid");

            public static Error ValueIsRequired() =>
                new Error("value.is.required", "Value is required");

            public static Error InvalidLength(string name = null)
            {
                string label = name == null ? " " : " " + name + " ";
                return new Error("invalid.string.length", $"Invalid{label}length");
            }

            public static Error Business(string message)
            {
                return new Error("business", message);
            }

            public static Error CollectionIsTooSmall(int min, int current)
            {
                return new Error(
                    "collection.is.too.small",
                    $"The collection must contain {min} items or more. It contains {current} items.");
            }

            public static Error CollectionIsTooLarge(int max, int current)
            {
                return new Error(
                    "collection.is.too.large",
                    $"The collection must contain {max} items or more. It contains {current} items.");
            }

            public static Error InternalServerError(string message)
            {
                return new Error("internal.server.error", message);
            }

            public static Error Unauthorized()
            {
                return new Error("unauthorized", "Credenciais inválidas");
            }
        }
    }
}
