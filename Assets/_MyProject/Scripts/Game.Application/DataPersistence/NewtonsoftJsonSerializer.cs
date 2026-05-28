using Newtonsoft.Json;

public class NewtonsoftJsonSerializer<Tdata> : IStringSerializer<Tdata>
{
    public Tdata Deserialize(string output) => JsonConvert.DeserializeObject<Tdata>(output);

    public string Serialize(Tdata data) => JsonConvert.SerializeObject(data);
}