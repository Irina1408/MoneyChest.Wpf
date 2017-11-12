using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MoneyChest.Utils.SerializationUtils
{
    public static class SerializationUtils
    {
        /// <summary>
        /// Returns string with serialized data of type T 
        /// </summary>
        public static string Serialize<T>(T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, data);
                result = writer.ToString();
            }
            return result;
        }

        /// <summary>
        /// Returns deserialized data of type T from string
        /// </summary>
        public static T Deserialize<T>(string data)
            where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            T result = null;
            try
            {
                using (TextReader reader = new StringReader(data))
                {
                    result = serializer.Deserialize(reader) as T;
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            return result;
        }
    }
}
