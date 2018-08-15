//using System;
//using System.Collections.Generic;
//using System.Linq;
//
//namespace Feree.Validator
//{
//    public abstract class ValidationRule<TError>
//    {
//        protected internal abstract bool IsValid();
//        protected internal abstract IEnumerable<TError> Errors { get; }
//
//        public IEnumerable<TError> Apply() => IsValid() == false
//            ? Errors
//            : Enumerable.Empty<TError>();
//    }
//
//    public class PredicateRule<TError> : ValidationRule<TError>
//    {
//        private readonly Func<bool> _predicate;
//
//        public PredicateRule(Func<bool> predicate, TError error)
//        {
//            _predicate = predicate;
//            Errors = new[] {error};
//        }
//
//        protected internal override bool IsValid() => _predicate();
//
//        protected internal override IEnumerable<TError> Errors { get; }
//    }
//
//    internal class ConditionalRule<TError> : ValidationRule<TError>
//    {
//        private readonly ValidationRule<TError> _rule;
//        private readonly Func<bool> _condition;
//
//        public ConditionalRule(ValidationRule<TError> rule, Func<bool> condition)
//        {
//            _rule = rule;
//            _condition = condition;
//        }
//
//        protected internal override bool IsValid() => !_condition() || _rule.IsValid();
//
//        protected internal override IEnumerable<TError> Errors => _rule.Errors;
//    }
//
//    public class NotNullRule<T, TError> : PredicateRule<TError> where T : class
//    {
//        public NotNullRule(Func<T> value, TError error) : base(() => !(value() is null), error)
//        {
//        }
//    }
//
//    public class Validator<TError>
//    {
//        private readonly Stack<ValidationRule<TError>> _validationRules =
//            new Stack<ValidationRule<TError>>();
//        
//        public Validator<TError> Must(Func<bool> predicate, TError error)
//        {
//            _validationRules.Push(new PredicateRule<TError>(predicate, error));
//            return this;
//        }
//        
//        public Validator<TError> Must(ValidationRule<TError> rule)
//        {
//            _validationRules.Push(rule);
//            return this;
//        }
//        
//        public Validator<TError> If(Func<bool> condition)
//        {
//            var rule = _validationRules.Pop();
//            _validationRules.Push(new ConditionalRule<TError>(rule, condition));
//            return this;
//        }
//
//        public IEnumerable<TError> Validate() => _validationRules.SelectMany(a => a.Apply());
//    }
//}