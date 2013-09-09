namespace OldPlugin
{
    using Runtime;
    using System;
    using System.Linq;

    [Key("old")]
    public class OldValueProvider : IValueProvider
    {
        public object GetValue()
        {
            return "old";
        }
    }
}