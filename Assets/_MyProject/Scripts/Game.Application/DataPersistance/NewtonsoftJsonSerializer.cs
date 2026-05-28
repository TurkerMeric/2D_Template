using Newtonsoft.Json;

public class NewtonsoftJsonSerializer<Tdata> : IDataSerializer<Tdata, string>
{
    public Tdata Deserialize(string output) => JsonConvert.DeserializeObject<Tdata>(output);

    public string Serialize(Tdata data) => JsonConvert.SerializeObject(data);
}