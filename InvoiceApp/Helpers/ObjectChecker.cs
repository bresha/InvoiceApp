using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Helpers
{
    public static class ObjectChecker
    {
        public static List<string> PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            var output = new List<string>();
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            output.Add(pi.Name);
                        }
                    }
                }

                return output;
            }

            throw new ArgumentNullException("One or both compared objects are null.");
        }
    }
}
