namespace NuPattern.ComponentModel
{
    using System;

    public interface IValueSerializer
    {
        string Serialize(object value);
        object Deserialize(string value);
    }
}