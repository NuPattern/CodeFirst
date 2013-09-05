namespace NuPattern
{
    using System;
    using System.Linq;

    internal static class NullProgress<T>
    {
        public static IProgress<T> Default { get; private set; }

        static NullProgress()
        {
            Default = new NullProgressImpl();
        }

        internal class NullProgressImpl : IProgress<T>
        {
            public void Report(T value)
            {
            }
        }
    }
}