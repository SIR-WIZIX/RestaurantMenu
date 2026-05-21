using System.IO;
using System.Xml.Serialization;

namespace Model.Data
{
    public class XmlSerializer<T> : BaseSerializer<T>
    {
        public override void Serialize(T data, string filePath)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        public override T Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
