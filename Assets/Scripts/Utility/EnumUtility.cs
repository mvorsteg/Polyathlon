using System;
using System.ComponentModel;

public class EnumUtility
{
    public static bool TryGetValueFromDescription<T>(string description, out T result)
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();
        foreach (var field in type.GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
            {
                if (attribute.Description == description)
                {
                    result = (T)field.GetValue(null);
                    return true;
                }
            }
            else
            {
                if (field.Name == description)
                {
                    result = (T)field.GetValue(null);
                    return true;
                }
            }
        }
        result = default;
        return false;
    }
}