namespace NuPattern
{
    using System;
    using System.Collections.Generic;

    public interface IAnnotated
    {
        void AddAnnotation(object annotation);
        object Annotation(Type type);
        IEnumerable<object> Annotations(Type type);
        void RemoveAnnotations(Type type);
    }
}