using System;
using Xunit;

namespace Feree.Validator.Tests
{
    public class Dog
    {
        public Dog(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public string Breed { get; }
    }

    public class HuskysNameMustBeLongerThan2 : ConditionalRule<string>
    {
        public static HuskysNameMustBeLongerThan2 Create(Func<string> name, Func<string> breed) =>
            new HuskysNameMustBeLongerThan2(name, breed);
        
        private HuskysNameMustBeLongerThan2(Func<string> name, Func<string> breed) 
            : base(new CustomRule<string, string>(name, n => n.Length > 2, "huskys name must be longer"),  () => breed() == "husky")
        {
        }
    }
    
    public class DogValidator : Validator<string>
    {
        
        
        public DogValidator(string name, string breed)
        {
            AddRule(RuleFactory.NotNull(() => name, "dogs name cannot be null"));
            AddRule(RuleFactory.NotNull(() => breed, "dogs breed cannot be null"));
            AddRule(HuskysNameMustBeLongerThan2.Create(() => name, () => breed));
        }
    }
    
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var dogsName = "bob";
            var dogsBreed = "random";
            
            var validator = new DogValidator(dogsName, dogsBreed);
            var errors = validator.Validate();

            Assert.Empty(errors);
        }
        
        [Fact]
        public void Test2()
        {
            var dogsName = "bob";
            var dogsBreed = "husky";
            
            var validator = new DogValidator(dogsName, dogsBreed);
            var errors = validator.Validate();

            Assert.Empty(errors);
        }
        
        [Fact]
        public void Test3()
        {
            var dogsName = "bo";
            var dogsBreed = "husky";
            
            var validator = new DogValidator(dogsName, dogsBreed);
            var errors = validator.Validate();

            Assert.Contains("huskys name must be longer", errors);
        }
    }
}