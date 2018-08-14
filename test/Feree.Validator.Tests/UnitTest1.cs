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
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var dogsName = "bob";

            var errors = new Validator<string>()
                .Must(new NotNull<string, string>(() => dogsName, "dogs name must not be null"))
                .Must(() => dogsName.Length > 2, "dogs name is too short")
                .If(() => true)
                .Validate();

            Assert.Empty(errors);
        }
        
        [Fact]
        public void Test2()
        {
            var dogsName = "bb";

            var errors = new Validator<string>()
                .Must(new NotNull<string, string>(() => dogsName, "dogs name must not be null"))
                .Must(() => dogsName.Length > 2, "dogs name is too short")
                .If(() => false)
                .Validate();

            Assert.Empty(errors);
        }
        
        [Fact]
        public void Test3()
        {
            var dogsName = "bb";

            var dogsNameIsTooShort = "dogs name is too short";
            var errors = new Validator<string>()
                .Must(new NotNull<string, string>(() => dogsName, "dogs name must not be null"))
                .Must(() => dogsName.Length > 2, dogsNameIsTooShort)
                .If(() => true)
                .Validate();

            Assert.Contains(dogsNameIsTooShort, errors);
        }
    }
}