namespace OldPlugin
{
    using Library;
    using Runtime;
    using System;
    using System.Linq;

    public class OldCommand : ICloneable
    {
        private IValueProvider provider;
        private IPatternElement element;
        private ConstantValueProvider constant;

        public OldCommand([Key("old")] IValueProvider provider, IPatternElement element, ConstantValueProvider constant)
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