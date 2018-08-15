using System;
using System.Collections.Generic;
using System.Linq;

namespace Feree.Validator
{
    public class RuleFactory {
        public static IRule<TError> NotNull<TError, TObject>(Func<TObject> p, TError e) where TObject : class 
            => new NotNullRule<TObject,TError>(p, e);
    }
    
    public abstract class Validator<TError>
    {
        private readonly HashSet<IRule<TError>> _rules = new HashSet<IRule<TError>>();

        protected void AddRule(IRule<TError> rule) => _rules.Add(rule);

        public IEnumerable<TError> Validate() => _rules.SelectMany(rule => rule.Apply());
    }

    public interface IRule<out TError>
    {
        IEnumerable<TError> Apply();
    }

    public class ConditionalRule<TError> : IRule<TError>
    {
        private readonly IRule<TError> _rule;
        private readonly Func<bool> _condition;

        public ConditionalRule(IRule<TError> rule, Func<bool> condition)
        {
            _rule = rule;
            _condition = condition;
        }

        public IEnumerable<TError> Apply() => _condition() ? _rule.Apply() : Enumerable.Empty<TError>();
    }

    public class NotNullRule<TObject, TError> : IRule<TError> where TObject : class
    {
        private readonly Func<TObject> _value;
        private readonly TError _error;

        public NotNullRule(Func<TObject> value, TError error)
        {
            _value = value;
            _error = error;
        }

        public IEnumerable<TError> Apply()
        {
            if (_value() is null)
                yield return _error;
        }
    }

    public class CustomRule<TObject, TError> : IRule<TError>
    {
        private readonly Func<TObject> _value;
        private readonly Func<TObject, bool> _predicate;
        private readonly TError _error;

        public CustomRule(Func<TObject> value, Func<TObject, bool> predicate, TError error)
        {
            _value = value;
            _predicate = predicate;
            _error = error;
        }

        public IEnumerable<TError> Apply()
        {
            if (!_predicate(_value()))
                yield return _error;
        }
    }
    
    public class CompositeRule<TError> : IRule<TError>
    {
        private readonly IEnumerable<IRule<TError>> _rules;
        private readonly TError _error;

        public CompositeRule(IEnumerable<IRule<TError>> rules, TError error)
        {
            _rules = rules;
            _error = error;
        }
        public IEnumerable<TError> Apply()
        {
            if (_rules.SelectMany(rule => rule.Apply()).Any())
                yield return _error;
        }
    }
}