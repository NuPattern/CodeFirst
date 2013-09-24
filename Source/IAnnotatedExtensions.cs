namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IAnnotatedExtensions
    {
        public static T Annotation<T>(this IAnnotated annotated) where T : class
        {
            return (T)annotated.Annotation(typeof(T));
        }

        public static IEnumerable<T> Annotations<T>(this IAnnotated annotated) where T : class
        {
            return annotated.Annotations(typeof(T)).OfType<T>();
        }

        public static void RemoveAnnotations<T>(this IAnnotated annotated) where T : class
        {
            annotated.RemoveAnnotations(typeof(T));
        }
    }
}