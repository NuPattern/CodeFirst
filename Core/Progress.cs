namespace NuPattern
{
    using System;

    public static class Progress
    {
        public static IProgress<T> Of<T>(Action<T> report)
        {
            return new AnonymousProgress<T>(report);
        }

        private class AnonymousProgress<T> : IProgress<T>
        {
            private Action<T> report;

            public AnonymousProgress(Action<T> report)
            {
                this.report = report;
            }

            public void Report(T value)
            {
                this.report(value);
            }
        }
    }
}
