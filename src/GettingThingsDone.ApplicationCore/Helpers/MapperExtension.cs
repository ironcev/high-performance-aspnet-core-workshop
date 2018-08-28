using System;
using System.Reflection;

namespace GettingThingsDone.ApplicationCore.Helpers
{
    /// <summary>
    /// Provides object extensions from mapping objects of one type to objects of a different type.
    /// </summary>
    /// <remarks>
    /// This is a very simple reflection based implementation of an object-to-object mapper.
    /// For a more sophisticated mapper see: https://automapper.org/
    /// </remarks>
    public static class MapperExtension
    {
        /// <summary>
        /// Creates an instance of an object of type <typeparamref name="TDestination"/> by copying
        /// the properties from the <paramref name="from"/> object. Properties are compared by name and
        /// type. 
        /// </summary>
        public static TDestination TranslateTo<TDestination>(this object from) where TDestination : new()
        {
            if (from == null) return default(TDestination);

            TDestination to = new TDestination();

            PropertyInfo[] properties = from.GetType().GetProperties();

            Type destinationType = to.GetType();
            foreach (PropertyInfo prop in properties)
            {
                PropertyInfo destinationProperty = destinationType.GetProperty(prop.Name);
                if (destinationProperty != null)
                {
                    Type destinationPropertyType = destinationProperty.PropertyType;
                    try
                    {
                        if (destinationPropertyType.IsEnum)
                        {
                            object enumValue = Enum.Parse(destinationPropertyType, prop.GetValue(from, null).ToString());
                            destinationProperty.SetValue(to, enumValue, null);
                        }
                        else
                        {
                            destinationProperty.SetValue(to, prop.GetValue(from, null), null);
                        }
                    }
                    catch { }
                }
            }

            return to;
        }

        /// <summary>
        /// Copies properties from the object <paramref name="from"/> to the object <paramref name="to"/>.
        /// Properties are compared by name and type. 
        /// </summary>
        /// <param name="to">The object to which to copy the properties.</param>
        /// <param name="from">The object from which to copy the properties.</param>
        public static TDestination CopyPropertiesFrom<TDestination>(this TDestination to, object from)
        {
            if (from == null || to == null) return to;

            PropertyInfo[] properties = to.GetType().GetProperties();

            Type fromType = from.GetType();
            foreach (PropertyInfo toProp in properties)
            {
                PropertyInfo fromProp = fromType.GetProperty(toProp.Name, toProp.PropertyType);
                if (fromProp != null)
                {
                    try
                    {
                        if (toProp.PropertyType.IsEnum)
                        {
                            object enumValue = Enum.Parse(toProp.PropertyType, fromProp.GetValue(from, null).ToString());
                            toProp.SetValue(to, enumValue, null);
                        }
                        else
                        {
                            toProp.SetValue(to, fromProp.GetValue(from, null), null);
                        }
                    }
                    catch { }
                }
            }

            return to;
        }
    }
}