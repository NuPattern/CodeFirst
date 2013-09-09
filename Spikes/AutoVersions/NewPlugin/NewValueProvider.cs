namespace NewPlugin
{
    using Runtime;
    using System;
    using System.Linq;

    [Key("old")]
    public class NewValueProvider : IValueProvider
    {
        public NewValueProvider(IPatternElement element)
        {
        }

        public object GetValue()
        {
            return "new";
        }
    }
}