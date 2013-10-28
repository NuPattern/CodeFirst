namespace NuPattern.Proxy
{
    using System;
    using System.Linq;

    public class Proxied : IProxied
    {
        public Proxied(IComponent component)
        {
            this.Component = component;
        }

        public IComponent Component { get; private set; }
    }
}