namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implements the behavior of annotated classes around an array of 
    /// annotations. Extracted from XObject implementation.
    /// </summary>
    internal static class Annotator
    {
        public static void AddAnnotation(ref object annotations, object annotation)
        {
            Guard.NotNull(() => annotation, annotation);

            if (annotations == null)
            {
                annotations = (annotation is object[]) ? new object[] { annotation } : annotation;
            }
            else
            {
                var annotationsArray = annotations as object[];
                if (annotationsArray == null)
                {
                    annotations = new object[] { annotations, annotation };
                }
                else
                {
                    int index = 0;
                    while ((index < annotationsArray.Length) && (annotationsArray[index] != null))
                    {
                        index++;
                    }
                    if (index == annotationsArray.Length)
                    {
                        Array.Resize<object>(ref annotationsArray, index * 2);
                        annotations = annotationsArray;
                    }
                    annotationsArray[index] = annotation;
                }
            }
        }

        public static object Annotation(object annotations, Type type)
        {
            Guard.NotNull(() => type, type);

            if (annotations != null)
            {
                var annotationsArray = annotations as object[];
                if (annotationsArray == null)
                {
                    if (type.IsInstanceOfType(annotations))
                        return annotations;
                }
                else
                {
                    for (int i = 0; i < annotationsArray.Length; i++)
                    {
                        var value = annotationsArray[i];
                        if (value == null)
                            break;

                        if (type.IsInstanceOfType(value))
                            return value;
                    }
                }
            }

            return null;
        }

        public static IEnumerable<object> Annotations(object annotations, Type type)
        {
            Guard.NotNull(() => type, type);

            if (annotations == null)
                yield break;

            var annotationsArray = annotations as object[];
            if (annotationsArray != null)
            {
                for (int i = 0; i < annotationsArray.Length; i++)
                {
                    var value = annotationsArray[i];
                    if (value == null)
                        break;

                    if (type.IsInstanceOfType(value))
                        yield return value;
                }

                yield break;
            }

            if (!type.IsInstanceOfType(annotations))
            {
                yield break;
            }

            yield return annotations;
        }

        public static void RemoveAnnotations(ref object annotations, Type type)
        {
            Guard.NotNull(() => type, type);

            if (annotations != null)
            {
                var annotationsArray = annotations as object[];
                if (annotationsArray == null)
                {
                    if (type.IsInstanceOfType(annotations))
                        annotations = null;
                }
                else
                {
                    int sourceIndex = 0;
                    int targetIndex = 0;
                    while (sourceIndex < annotationsArray.Length)
                    {
                        var value = annotationsArray[sourceIndex];
                        if (value == null)
                            break;

                        if (!type.IsInstanceOfType(value))
                            annotationsArray[targetIndex++] = value;

                        sourceIndex++;
                    }

                    if (targetIndex != 0)
                    {
                        while (targetIndex < sourceIndex)
                            annotationsArray[targetIndex++] = null;
                    }
                    else
                    {
                        annotations = null;
                    }
                }
            }
        }
    }
}