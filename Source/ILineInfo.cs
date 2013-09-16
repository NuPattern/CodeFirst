namespace NuPattern
{
    using System;

    /// <summary>
    /// For products that have been deserialized from a state file, 
    /// represents the line and column information of the component.
    /// </summary>
    public interface ILineInfo
    {
        bool HasLineInfo { get; }
        int? LinePosition { get; }
        int? LineNumber { get; }
    }
}