using System;
using System.Collections.Generic;

namespace validator
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

        public static ValidatonResult CreateSuccess() => new ValidatonResult.Success();
        public static ValidatonResult CreateFailure<T>(IEnumerable<T> errors) => new ValidatonResult.Failure<T>(errors);
        public static ValidatonResult CreateFailure<T>(T error) => new ValidatonResult.Failure<T>(new[] { error });
    }

    public class Rule<T>
    {
        private readonly Func<T> _valueExtractor;
        private Func<T, bool> _validationFuncion = _ => true;
        private Func<bool> _conditionFunction = () => true;
        private string _errorMessage = "validation failed";
        private Rule(Func<T> valueExtractor) => _valueExtractor = valueExtractor;

        public static Rule<T> For(Func<T> valueExtractor) => new Rule<T>(valueExtractor);

        public Rule<T> Must(Func<T, bool> predicate)
        {
            _validationFuncion = predicate;
            return this;
        }

        public Rule<T> Message(string message)
        {
            _errorMessage = message;
            return this;
        }

        public Rule<T> When(Func<bool> condition) 
        {
            _conditionFunction = condition;
            return this;
        }

        public Rule<T> When(Func<T, bool> condition) 
        {
            _conditionFunction = () => condition(_valueExtractor());
            return this;
        }

        public ValidatonResult Apply() => 
            _validationFuncion == null || _validationFuncion(_valueExtractor())
                ? ValidatonResult.CreateSuccess()
                : ValidatonResult.CreateFailure(new[] { _errorMessage });
    }

    public class poc
    {
        public decimal price { get; }

        public poc(decimal price)
        {
            var result = Rule<decimal>
                            .For(() => price)
                            .Must(x => x > 0)
                            .When(x => x != -5)
                            .Message("you fucker")
                            .Apply();
        }
    }
}
