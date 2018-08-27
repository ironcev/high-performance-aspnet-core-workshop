using System;
using System.Collections.Generic;
using System.Text;

namespace GettingThingsDone.ApplicationCore.Helpers
{
    public static class MapperExstension
    {
        /// <summary>
        /// Kreiranje instance nekog objekta na način da se kopiraju propertyji iz nekog drugog tipa.
        /// Propertyji se kopiraju iz <see cref="from"/> u novu instancu <see cref="TDest"/>.
        /// Propertiji se uspoređuju po nazivu (kopiraju se samo oni propertyji koji postoje u oba tipa).
        /// </summary>
        /// <typeparam name="TDest"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        public static TDest TranslateTo<TDest>(this object from) where TDest : new()
        {
            if (from == null)
            {
                return default(TDest);
            }

            TDest to = new TDest();

            System.Reflection.PropertyInfo[] properties = from.GetType().GetProperties();

            Type destType = to.GetType();
            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                System.Reflection.PropertyInfo destProp = destType.GetProperty(prop.Name);
                if (destProp != null)
                {
                    Type destPropType = destProp.PropertyType;
                    try
                    {
                        if (destPropType.IsEnum)
                        {
                            object enumvalue = Enum.Parse(destPropType, prop.GetValue(from, null).ToString());
                            if (enumvalue != null)
                            {
                                destProp.SetValue(to, enumvalue, null);
                            }
                        }
                        else
                        {
                            destProp.SetValue(to, prop.GetValue(from, null), null);
                        }
                    }
                    catch { }
                }
            }

            return to;
        }

        /// <summary>
        /// Kopiranje propertija sa istim nazivom i tipom podatka iz jednog objekta u drugi.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        public static void CopyProperties(this object to, object from)
        {
            if (from == null || to == null)
            {
                return;
            }

            System.Reflection.PropertyInfo[] properties = to.GetType().GetProperties();

            Type fromType = from.GetType();
            foreach (System.Reflection.PropertyInfo toProp in properties)
            {
                System.Reflection.PropertyInfo fromProp = fromType.GetProperty(toProp.Name, toProp.PropertyType);
                if (fromProp != null)
                {
                    try
                    {
                        if (toProp.PropertyType.IsEnum)
                        {
                            object enumvalue = Enum.Parse(toProp.PropertyType, fromProp.GetValue(from, null).ToString());
                            if (enumvalue != null)
                            {
                                toProp.SetValue(to, enumvalue, null);
                            }
                        }
                        else
                        {
                            toProp.SetValue(to, fromProp.GetValue(from, null), null);
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
