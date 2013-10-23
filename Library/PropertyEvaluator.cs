namespace NuPattern
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Implements the default <see cref="IPropertyEvaluator"/> behavior.
    /// </summary>
    public class PropertyEvaluator : IPropertyEvaluator
    {
        /// <summary>
        /// Evaluates the property for the given target object.
        /// </summary>
        /// <returns>
        /// The property value or <see langword="null"/> if the property is not found
        /// </returns>
        public object Evaluate(object target, string propertyName)
        {
            if (target == null)
                return null;

            // Try via TypeDescriptor
            var property = TypeDescriptor.GetProperties(target)[propertyName];
            if (property != null)
                return property.GetValue(target);

            // Try via toolkit interface.
            var component = target as IComponent;
            if (component != null)
            {
                var prop = component.Properties.FirstOrDefault(p => p.Name == propertyName);
                if (prop != null)
                    return prop.Value;
            }

            // Try via reflection (supported internal properties for internal DSL classes)
            var propInfo = target.GetType().GetProperty(propertyName, (BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static));
            if (propInfo != null)
                return propInfo.GetValue(target, null);

            return null;
        }
    }
}
