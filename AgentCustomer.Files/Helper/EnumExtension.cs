
using System.ComponentModel;
using System.Reflection;
 
namespace AgentCustomer.Files
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum element)
        {
            Type type = element.GetType();

            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return element.ToString();
        }
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }

        public static IEnumerable<string> GetNames<T>()
        {
            var descriptionList = new List<string>();
            var values = Enum.GetValues(typeof(T));
            foreach (var item in values)
            {
                if (((Enum)item).GetDescription() != "Empty")//default value not needed in UI display
                {
                    descriptionList.Add(((Enum)item).GetDescription());
                }
            }

            return descriptionList;
        }

        public static Dictionary<string, string> GetNameAndDescription<T>()
        {
            var descriptionList = new Dictionary<string, string>();
            var values = Enum.GetValues(typeof(T));
            foreach (var item in values)
            {
                if (((Enum)item).GetDescription() != "Empty")//default value not needed in UI display
                {
                    descriptionList.Add(item.ToString(), ((Enum)item).GetDescription());
                }
            }

            return descriptionList;
        }

        public static T GetValueFromDescription<T>(string description, T orDefault)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                        return (T)field.GetValue(null);
                }
            }

            return orDefault;
        }

        public static T ParseEnum<T>(string value, T defaultEnum)
        {
            if (Enum.TryParse(typeof(T), value, out var parsedValue))
            {
                return (T)parsedValue;
            }
            else return defaultEnum;
        }
    }
}
