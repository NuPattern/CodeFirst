namespace Runtime
{
    using System;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public class KeyAttribute : Attribute
    {
        public KeyAttribute(string key)
        {
            this.Key = key;
        }

        public string Key { get; private set; }
    }
}