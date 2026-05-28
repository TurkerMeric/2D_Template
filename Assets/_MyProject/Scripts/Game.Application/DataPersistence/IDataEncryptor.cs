public interface IDataEncryptor<Tformat>
{
    Tformat Encrypt(Tformat data);
    Tformat Decrypt(Tformat data);
}
