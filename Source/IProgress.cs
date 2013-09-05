namespace NuPattern
{
    public interface IProgress<in T>
    {
        void Report(T value);
    }
}