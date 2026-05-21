using System.IO;
using System.Text.Json;

namespace Model.Data
{
    public class JsonSerializer<T> : BaseSerializer<T>
    {
        public override void Serialize(T data, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(data, options);

            File.WriteAllText(filePath, jsonString);
        }

        public override T Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            string jsonString = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
