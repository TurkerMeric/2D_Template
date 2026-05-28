public class NoEncryption : IDataEncryptor
{
    public string Decrypt(string data) => data;

    public string Encrypt(string data) => data;
}
