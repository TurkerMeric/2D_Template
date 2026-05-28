public interface IDataSerializer<Tin, Tout>
{
    public Tout Serialize(Tin data);
    public Tin Deserialize(Tout output);
}
