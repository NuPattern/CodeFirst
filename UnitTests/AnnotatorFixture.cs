namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class AnnotatorFixture
    {
        [Fact]
        public void when_adding_annotation_then_state_equals_annotation()
        {
            var annotated = new AnnotatedObject();
            var annotation = new FooAnnotation();

            annotated.AddAnnotation(annotation);

            Assert.Same(annotation, annotated.AnnotationState);
        }

        [Fact]
        public void when_adding_annotation_then_can_retrieve_it()
        {
            var annotated = new AnnotatedObject();

            annotated.AddAnnotation(new FooAnnotation());

            Assert.NotNull(annotated.Annotation<FooAnnotation>());
        }

        [Fact]
        public void when_adding_second_annotation_then_state_becomes_array()
        {
            var annotated = new AnnotatedObject();

            annotated.AddAnnotation(new FooAnnotation());
            annotated.AddAnnotation(new BarAnnotation());

            Assert.IsType<object[]>(annotated.AnnotationState);
        }

        [Fact]
        public void when_adding_two_annotations_then_can_retrieve_one()
        {
            var annotated = new AnnotatedObject();
            var annotation = new FooAnnotation();

            annotated.AddAnnotation(annotation);
            annotated.AddAnnotation(new BarAnnotation());

            Assert.Same(annotation, annotated.Annotation<FooAnnotation>());            
        }

        [Fact]
        public void when_adding_two_annotations_of_same_type_then_can_retrieve_them()
        {
            var annotated = new AnnotatedObject();

            annotated.AddAnnotation(new FooAnnotation());
            annotated.AddAnnotation(new BarAnnotation());
            annotated.AddAnnotation(new FooAnnotation());

            var annotations = annotated.Annotations<FooAnnotation>().ToList();

            Assert.Equal(2, annotations.Count);
        }

        [Fact]
        public void when_last_annotation_removed_then_sets_state_to_null()
        {
            var annotated = new AnnotatedObject();

            annotated.AddAnnotation(new FooAnnotation());
            annotated.AddAnnotation(new BarAnnotation());

            annotated.RemoveAnnotations<FooAnnotation>();
            annotated.RemoveAnnotations<BarAnnotation>();

            Assert.Null(annotated.AnnotationState);
        }

    }

    public class FooAnnotation { }
    public class BarAnnotation { }

    public class AnnotatedObject : IAnnotated
    {
        private object annotations;

        public object AnnotationState { get { return annotations; } }

        public void AddAnnotation(object annotation)
        {
            Annotator.AddAnnotation(ref annotations, annotation);
        }

        public object Annotation(Type type)
        {
            return Annotator.Annotation(annotations, type);
        }

        public IEnumerable<object> Annotations(Type type)
        {
            return Annotator.Annotations(annotations, type);
        }

        public void RemoveAnnotations(Type type)
        {
            Annotator.RemoveAnnotations(ref annotations, type);
        }
    }

}