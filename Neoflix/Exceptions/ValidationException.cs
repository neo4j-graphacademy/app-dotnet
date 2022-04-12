using System;

namespace Neoflix.Exceptions
{
    public class ValidationException : Exception
    {
        public readonly int Code = 422;
        public string Details { get; init; }

        public ValidationException(string message, string details)
            : base(message)
        {
            Details = details;
        }
    }
}
