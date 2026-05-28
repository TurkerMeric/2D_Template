public interface IStringSerializer<T>
{
    string Serialize(T data);
    T Deserialize(string json);
}