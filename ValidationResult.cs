using System.Collections.Generic;

namespace Feree.Validator
{
    public abstract class ValidatonResult
    {
        public class Success : ValidatonResult
        {
            protected internal Success() { }
        }

        public class Failure<T> : ValidatonResult
        {
            public IEnumerable<T> Errors { get; }
            protected internal Failure(IEnumerable<T> errors) => Errors = errors;
        }

        public static ValidatonResult CreateSuccess() =>
            new ValidatonResult.Success();
        public static ValidatonResult CreateFailure<T>(IEnumerable<T> errors) =>
            new ValidatonResult.Failure<T>(errors);
        public static ValidatonResult CreateFailure<T>(T error) =>
            new ValidatonResult.Failure<T>(new[] { error });
    }
}