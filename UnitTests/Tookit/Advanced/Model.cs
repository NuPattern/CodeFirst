namespace NuPattern.Tookit.Advanced
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum Visibility
    {
        Hidden,
        Visible,
        Content
    }

    public class VisibilityAttribute
    {
        public VisibilityAttribute()
        {

        }
    }


    // Product
    public interface IAws
    {
        // Primitive properties
        string AccessKey { get; set; }
        string SecretKey { get; set; }
        // Direct element reference 1..1, auto-create most likely
        IStorage Storage { get; set; }
    }

    // Element
    public interface IStorage
    {
        // Direct collection reference, 1..1 auto-create most likely
        IBuckets Buckets { get; set; }

        // Whether to display or not the containing property name
        // as a node (non-delete-able in this case.
        // An attribute like DesignerSerializationVisibility with Hidden, Visible and Content
        // would be very useful here. 
        // Hidden: the property and its children are not shown on the tree
        // Visible: the property and its children are shown
        // Content: only the children are shown, not the property node.
        IEnumerable<IBuckets> BucketGroups { get; }
    }

    // Collection with properties and child elements.
    // Note: in original NuPattern, collections could contain 
    // arbitrary element types, here it looks we're limiting 
    // to just one, unless we implement multiple IEnumerable<T>...
    public interface IBuckets : IEnumerable<IBucket>
    {
        // Collection primitive property
        bool RefreshOnLoad { get; set; }
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