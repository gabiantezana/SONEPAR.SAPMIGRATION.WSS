using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public class ValidationHelper
    {
        private List<String> errors = new List<String>();

        public void Validate(object o)
        {
            Type type = o.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Type attrType = typeof(ValidationAttribute);

            foreach (var propertyInfo in properties)
            {
                Object propValue = propertyInfo.GetValue(o, null);
                if (propValue != null)
                    if (propValue.GetType().IsGenericType)
                        foreach (var item in (IEnumerable)propValue)
                        {
                            Validate(item);
                        }
                    else if (propertyInfo.PropertyType.Assembly == type.Assembly)
                        Validate(propValue);

                object[] customAttributes = propertyInfo.GetCustomAttributes(attrType, inherit: true);
                foreach (var customAttribute in customAttributes)
                {
                    var validationAttribute = (ValidationAttribute)customAttribute;
                    bool isValid = validationAttribute.IsValid(propertyInfo.GetValue(o, BindingFlags.GetProperty, null, null, null));
                    if (!isValid)
                        errors.Add(validationAttribute.FormatErrorMessage(propertyInfo.Name));
                }
            }
            if (errors.Count > 0)
                throw new Exception(String.Join(" | ", errors.ToArray()));
        }

    }
}
