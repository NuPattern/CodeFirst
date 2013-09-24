namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;

    public class BindingSettingsFixture
    {
        [Fact]
        public void when_resolving_binding_then_resolves_type()
        {
            var settings = new BindingSettings(typeof(GenerateCode))
            {
                Properties = 
                {
                    new ProvidedPropertySettings("TargetFileName")
                    {
                        Provider = new BindingSettings(typeof(ExpressionValueProvider))
                        {
                            Properties = 
                            {
                                new ProvidedPropertySettings("Expression")
                                {
                                    Provider = new BindingSettings(typeof(ExpressionValueProvider))
                                    {
                                        Properties = 
                                        {
                                            new ConstantPropertySettings("Expression", "{Name}Controller")
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new ConstantPropertySettings("TargetPath", "~"),
                }
            };

            var context = new ComponentContext();
            var binding = settings.Resolve(context);

            var command = (GenerateCode)binding.Instance;

            binding.Refresh();

            Assert.Equal("~", command.TargetPath);
            Assert.Equal("{Name}Controller11", command.TargetFileName);

            binding.Refresh();

            Assert.Equal("~", command.TargetPath);

            Assert.Equal("{Name}Controller22", command.TargetFileName);
        }
    }

    public class ExecuteFor<T> { }

    public static class GenerateCodeExtension
    {
        public static void GenerateCode<T>(this ExecuteFor<T> execute, Expression<Func<GenerateCode, bool>> initializer)
        {

        }
    }

    public class GenerateCode
    {
        public string TargetFileName { get; set; }
        public string TargetPath { get; set; }
    }

    public interface IValueProvider
    {
        object GetValue();
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

    public class Binding
    {
        public Binding(object instance, IEnumerable<PropertyBinding> properties)
        {
            this.Instance = instance;
            this.Properties = properties.ToArray();
        }

        public object Instance { get; private set; }
        public IEnumerable<PropertyBinding> Properties { get; private set; }

        public void Refresh()
        {
            foreach (var property in Properties)
            {
                property.Refresh(Instance);
            }
        }

        public override string ToString()
        {
            return Instance.ToString() + ": " + string.Join(", ", Properties.Select(x => x.ToString()));
        }
    }

    public abstract class PropertyBinding
    {
        public abstract void Refresh(object instance);
    }

    public class ConstantPropertyBinding : PropertyBinding
    {
        private Type instanceType;
        private string propertyName;
        private object value;

        public ConstantPropertyBinding(Type instanceType, string propertyName, object value)
        {
            this.instanceType = instanceType;
            this.propertyName = propertyName;
            this.value = value;
        }

        public override void Refresh(object instance)
        {
            instanceType.GetProperty(propertyName).SetValue(instance, value, null);
        }

        public override string ToString()
        {
            var valueString = value.ToString();
            if (value is string)
                valueString = "\"" + valueString + "\"";

            return propertyName + "=" + valueString;
        }
    }

    public class ProvidedPropertyBinding : PropertyBinding
    {
        private Type instanceType;
        private string propertyName;
        private Binding provider;

        public ProvidedPropertyBinding(Type instanceType, string propertyName, Binding providerBinding)
        {
            this.instanceType = instanceType;
            this.propertyName = propertyName;
            this.provider = providerBinding;
        }

        public override void Refresh(object instance)
        {
            provider.Refresh();
            instanceType.GetProperty(propertyName).SetValue(instance, ((IValueProvider)provider.Instance).GetValue(), null);
        }

        public override string ToString()
        {
            return propertyName + "={" + provider.ToString() + "}";
        }
    }

    public class BindingSettings
    {
        public BindingSettings()
        {
            this.Properties = new List<PropertySettings>();
        }

        public BindingSettings(Type type)
            : this()
        {
            this.Type = type;
        }

        public Type Type { get; set; }

        public IList<PropertySettings> Properties { get; set; }

        public Binding Resolve(IComponentContext context)
        {
            return new Binding(context.Instantiate(Type),
                Properties.Select(x => x.Resolve(Type, context)));
        }
    }

    public abstract class PropertySettings
    {
        public PropertySettings()
        {
        }

        public PropertySettings(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public abstract PropertyBinding Resolve(Type boundType, IComponentContext context);
    }

    public class ConstantPropertySettings : PropertySettings
    {
        public ConstantPropertySettings(string propertyName, object constantValue)
            : base(propertyName)
        {
            this.Value = constantValue;
        }

        public object Value { get; set; }

        public override PropertyBinding Resolve(Type boundType, IComponentContext context)
        {
            return new ConstantPropertyBinding(boundType, PropertyName, Value);
        }
    }

    public class ProvidedPropertySettings : PropertySettings
    {
        public ProvidedPropertySettings(string propertyName)
            : base(propertyName)
        {
        }

        public BindingSettings Provider { get; set; }

        public override PropertyBinding Resolve(Type boundType, IComponentContext context)
        {
            return new ProvidedPropertyBinding(boundType, PropertyName, Provider.Resolve(context));
        }
    }
}