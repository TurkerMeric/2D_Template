public class NoEncryption<Tformat> : IDataEncryptor<Tformat>
{
    public Tformat Decrypt(Tformat data) => data;

    public Tformat Encrypt(Tformat data) => data;

}
