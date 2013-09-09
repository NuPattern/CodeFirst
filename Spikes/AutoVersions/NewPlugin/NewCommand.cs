namespace NewPlugin
{
    using Library;
using Runtime;
using System;
using System.Linq;

    public class NewCommand : ICloneable
    {
        private IValueProvider provider;
        private IPatternElement element;
        private ConstantValueProvider constant;

        public NewCommand([Key("new")] IValueProvider provider, IPatternElement element, ConstantValueProvider constant)
        {
            this.provider = provider;
            this.element = element;
            this.constant = constant;
        }

        public object Clone()
        {
            return provider.GetValue() + "::runtime-" + element.GetType().Assembly.GetName().Version + "::library-" + constant.GetValue();
        }
    }
}