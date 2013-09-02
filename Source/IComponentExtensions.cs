namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IComponentExtensions
    {
        public static IEnumerable<IComponent> Ancestors(this IComponent @this)
        {
            var parent = @this.Parent;
            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }
    }
}