using System;

namespace Feree.Validator
{
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
}