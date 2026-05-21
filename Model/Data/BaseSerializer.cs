using System.IO;

namespace Model.Data
{
    public abstract class BaseSerializer<T>
    {
        public abstract void Serialize(T data, string filePath);

        public abstract T Deserialize(string filePath);
    }
}
