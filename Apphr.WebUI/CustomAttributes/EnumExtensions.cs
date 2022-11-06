using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Apphr.WebUI.CustomAttributes
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enu)
        {
            //return enu.GetType()
            //           .GetMember(enu.ToString())
            //           .First()
            //           .GetCustomAttribute<DisplayAttribute>()
            //           .GetName();
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }

        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
        //    public static string GetDisplayName(this Enum enumValue)
        //    {
        //        string displayName;
        //        displayName = enumValue.GetType()
        //            .GetMember(enumValue.ToString())
        //            .FirstOrDefault()
        //            .GetCustomAttribute<DisplayAttribute>()?
        //            .GetName();
        //        if (String.IsNullOrEmpty(displayName))
        //        {
        //            displayName = enumValue.ToString();
        //        }
        //        return displayName;
        //    }
    }
}