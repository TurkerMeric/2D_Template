using System.Text;

public class XorEncryptor : IDataEncryptor<string>
{
    private readonly string _key;

    // Şifreleme anahtarını dışarıdan alırız (Örn: "MySecretGameKey123")
    public XorEncryptor(string key)
    {
        _key = key;
    }

    public string Encrypt(string data)
    {
        return XorOperation(data);
    }

    public string Decrypt(string data)
    {
        return XorOperation(data); // XOR'un doğası gereği şifre çözme işlemi de aynı fonksiyonla yapılır
    }

    private string XorOperation(string input)
    {
        StringBuilder output = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            // Veriyi anahtarımızdaki karakterlerle XOR işlemine sokuyoruz
            output.Append((char)(input[i] ^ _key[i % _key.Length]));
        }
        return output.ToString();
    }
}