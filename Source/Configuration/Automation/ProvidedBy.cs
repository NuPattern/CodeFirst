namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class ProvidedBy
    {
        static ProvidedBy()
        {
            Default = new ProvidedBy();
        }

        private ProvidedBy()
        {
        }

        public static ProvidedBy Default { get; private set; }
    }
}