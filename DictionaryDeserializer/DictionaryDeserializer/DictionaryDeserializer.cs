using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace DictionaryDeserializer
{
    public class DictionaryDeserializer 
    {

        public T DeserializeFrom<T>(Dictionary<string, string> source) where T : new()
        {
            T instance = new T();
            foreach(string propertyName in source.Keys)
            {
                TrySetPropertyOrField(propertyName, source[propertyName], instance);
            }



            return instance;
        }

        protected static object? ParseStringTo(string source, Type targetType)
        {
            source = source.Replace('.', ',');
            TypeConverter converter = TypeDescriptor.GetConverter(targetType);
            if(converter == null) throw new NullReferenceException();
            var result = converter.ConvertFrom(source);
            return result;
        }

        protected bool TrySetPropertyOrField(string propertyName, string value, object target)
        {
            return (TrySetField(propertyName, value, target) || TrySetProperty(propertyName, value, target));
        }

        protected bool TrySetProperty(string propertyName, string value, object target)
        {
            PropertyInfo? property = target.GetType().GetProperty(propertyName);
            if (property == null) return false;
            Type propertyType = property.PropertyType;

            try
            {
            property.SetValue(target, ParseStringTo(value, propertyType));
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        protected bool TrySetField(string propertyName, string value, object target)
        {
            FieldInfo? property = target.GetType().GetField(propertyName);
            if (property == null) return false;
            Type fieldType = property.FieldType;
            
            try
            {
            property.SetValue(target, ParseStringTo(value, fieldType));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    
    }
}