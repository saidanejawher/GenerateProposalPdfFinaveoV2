using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaveoV2_PDF
{
    public static class EnumHelper
    {
        public static string ToDescription<T>(this T enumValue) where T : struct
        {
            return GetEnumDescription((Enum)(object)((T)enumValue));
        }
        private static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
