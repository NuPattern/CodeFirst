namespace NuPattern.Binding
{
    using NuPattern.Configuration;
    using System;
    using System.Linq;
    using Xunit;

    public class BindingFixture
    {
        [Fact]
        public void when_resolving_binding_then_resolves_type()
        {
            var config = new BindingConfiguration(typeof(GenerateCode))
            {
                Properties = 
                {
                    new PropertyBindingConfiguration("TargetFileName")
                    {
                        ValueProvider = new BindingConfiguration(typeof(ExpressionValueProvider))
                        {
                            Properties = 
                            {
                                new PropertyBindingConfiguration("Expression")
                                {
                                    ValueProvider = new BindingConfiguration(typeof(ExpressionValueProvider))
                                    {
                                        Properties = 
                                        {
                                            new PropertyBindingConfiguration("Expression")
                                            {
                                                Value = "{Name}Controller",
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new PropertyBindingConfiguration("TargetPath")
                    {
                        Value = "~",
                    },
                }
            };

            var context = new ComponentContext();
            var factory = new BindingFactory();
            var binding = factory.CreateBinding<GenerateCode>(context, config);

            var command = (GenerateCode)binding.Instance;

            binding.Refresh();

            Assert.Equal("~", command.TargetPath);
            Assert.Equal("{Name}Controller11", command.TargetFileName);

            binding.Refresh();

            Assert.Equal("~", command.TargetPath);

            Assert.Equal("{Name}Controller22", command.TargetFileName);
        }
    }

    public class GenerateCode
    {
        public string TargetFileName { get; set; }
        public string TargetPath { get; set; }
    }

    public class ExpressionValueProvider : IValueProvider
    {
        private int count;

        public ExpressionValueProvider()
        {
        }

        public ExpressionValueProvider(string expression)
        {
            this.Expression = expression;
        }

        public string Expression { get; set; }

        public object GetValue()
        {
            return Expression + ++count;
        }

        public override string ToString()
        {
            return Expression + count;
        }
    }
}