using System;

namespace Feree.Validator
{
    public class Rule<T>
    {
        private readonly Func<T> _valueExtractor;
        private readonly Func<T, bool> _validationFunction;
        private readonly Func<bool> _conditionFunction;
        private readonly string _errorMessage;
    
        private Rule(Func<T> valueExtractor, Func<T, bool> validationFunction, Func<bool> conditionFunction, string errorMessage)
        {
            _valueExtractor = valueExtractor;
            _validationFunction = validationFunction;
            _conditionFunction = conditionFunction;
            _errorMessage = errorMessage;
        }

        public static Rule<T> For(Func<T> valueExtractor) =>
            new Rule<T>(valueExtractor, _ => true, () => true, "validation failed");
        public Rule<T> Must(Func<T, bool> validationFunction) =>
            new Rule<T>(_valueExtractor, validationFunction, _conditionFunction, _errorMessage);

        public Rule<T> Message(string errorMessage) =>
            new Rule<T>(_valueExtractor, _validationFunction, _conditionFunction, errorMessage);

        public Rule<T> When(Func<bool> conditionFunction) =>
            new Rule<T>(_valueExtractor, _validationFunction, conditionFunction, _errorMessage);

        public Rule<T> When(Func<T, bool> conditionFunction) => 
            new Rule<T>(_valueExtractor, _validationFunction, () => conditionFunction(_valueExtractor()), _errorMessage);

        public ValidatonResult Apply() =>
            _validationFunction == null || _validationFunction(_valueExtractor())
                ? ValidatonResult.CreateSuccess()
                : ValidatonResult.CreateFailure(new[] { _errorMessage });
    }
}