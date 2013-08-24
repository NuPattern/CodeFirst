namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public enum Cardinality
    {
        OneToOne = 0,
        ZeroToOne = 1,
        OneToMany = 2,
        ZeroToMany = 3,
    }
}