using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBookDataLib.Common
{
    public static class DataAttributeExtension
    {
        public static Type GetOrDefault<Type>(this Type dataAttribute, Type defaultValue)
        {
            return dataAttribute != null ? dataAttribute : defaultValue;
        }
    }
}
