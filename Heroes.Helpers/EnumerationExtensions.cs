using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Heroes.Helpers
{
    public static class EnumerationExtensions
    {
        /// <summary>
        /// Returns the friendly name of the enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerationValue"></param>
        /// <returns></returns>
        public static string GetFriendlyName<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            // Tries to find a DescriptionAttribute for a potential friendly name for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return enumerationValue.ToString();
        }

        /// <summary>
        /// Convert the string to an Enumeration type
        /// </summary>
        /// <typeparam name="T">The Enumeration type to be converted to</typeparam>
        /// <param name="value">The stirng to be converted</param>
        /// <returns></returns>
        public static T ConvertToEnum<T>(this string value)
            where T : struct
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            value = Regex.Replace(value, @"\s+", string.Empty);

            if (Enum.TryParse(value, true, out T replayParseResultEnum))
                return replayParseResultEnum;
            else
                throw new ArgumentException($"paramter {value} not found", nameof(value));
        }
    }
}
