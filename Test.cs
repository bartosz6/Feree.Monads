using System;
using System.Collections.Generic;
using Feree.Validator;

namespace validator
{
    public class poc
    {
        public decimal price { get; }

        public poc(decimal price)
        {
            var result = Rule<decimal>
                            .For(() => price)
                            .Must(x => x > 0)
                            .When(x => x != -5)
                            .Message("error");
                            //.Apply();

                        var rule2 = new Rule<string>();

            var success = result is ValidatonResult.Success;
        }
    }
}
