namespace NuPattern.Tookit.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Product
    public interface IAmazonWebServices : IProduct
    {
        // Primitive properties
        string AccessKey { get; set; }
        string SecretKey { get; set; }

        // Implicit Element property. 1..1, default auto-create likely
        IStorage Storage { get; set; }
    }

    // Element
    public interface IStorage
    {
        // Element primitive property
        bool RefreshOnLoad { get; set; }

        // Implicit Collection property. 0..n, no default auto-create likely
        // IBucket (T) implicit Element.
        IEnumerable<IBucket> Buckets { get; }
    }

    // Element
    public interface IBucket : INamed
    {
        // Element primitive properties
        Permissions Permissions { get; set; }
    }

    public enum Permissions
    {
        ReadOnly,
        ReadWrite,
    }
}