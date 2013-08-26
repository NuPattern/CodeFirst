namespace NuPattern.Tookit.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Product
    public interface IAmazonWebServices : IPattern
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
    public interface IBucket
    {
        // Element primitive properties
        string Name { get; set; }
        Permissions Permissions { get; set; }
        // Element list property, 0..n most likely.
        IEnumerable<IObject> Objects { get; set; }
    }

    public interface IObject
    {
        string Key { get; set; }
        byte[] Value { get; set; }
        // Element-level property override from IBucket.Permissions.
        Permissions Permissions { get; set; }

        // Pointer to the parent node, should be a Reference, rather than a 
        // contained element. Denoted by it being read-only. Will need a value 
        // provider to get its value.
        // Should show up in the properties window only, not as a 
        // nested node? Typically hidden also... maybe these are 
        // default conventions?
        IBucket Bucket { get; }
    }

    public enum Permissions
    {
        ReadOnly,
        ReadWrite,
    }
}