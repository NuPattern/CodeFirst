namespace NuPattern
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    internal class SemanticVersionTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            SemanticVersion version;
            string str = value as string;
            if ((str != null) && SemanticVersion.TryParse(str, out version))
            {
                return version;
            }
            return null;
        }
    }
}