namespace NuPattern.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IBinding<out T> : IBinding
    {
        T Instance { get; }
    }

    public interface IBinding 
    {
        void Refresh();
    }
}