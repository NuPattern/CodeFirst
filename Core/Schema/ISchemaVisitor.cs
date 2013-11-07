namespace NuPattern.Schema
{
    using System;

    public interface ISchemaVisitor
    {
        bool VisitEnter(IToolkitSchema toolkit);
        bool VisitLeave(IToolkitSchema toolkit);

        bool VisitEnter(IProductSchema product);
        bool VisitLeave(IProductSchema product);

        bool VisitEnter(IElementSchema element);
        bool VisitLeave(IElementSchema element);

        bool VisitEnter(ICollectionSchema collection);
        bool VisitLeave(ICollectionSchema collection);
        
        bool VisitProperty(IPropertySchema property);
    }
}