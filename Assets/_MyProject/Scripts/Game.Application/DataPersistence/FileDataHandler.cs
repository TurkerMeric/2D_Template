using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler<Tdata, Tformat> : IDataHandler<Tdata>
{
    private readonly IDataSerializer<Tdata, Tformat> _dataSerializer;
    private readonly IDataEncryptor<Tformat> _dataEncryptor;
    private readonly string _directory;
    private readonly string _fileExtension;

    public FileDataHandler(IDataSerializer<Tdata, Tformat> dataSerializer, IDataEncryptor<Tformat> dataEncryptor)
    {
        _dataSerializer = dataSerializer;
        _dataEncryptor = dataEncryptor;
        _directory = Application.persistentDataPath;

        // Format tipine göre dosya uzantısını otomatik belirliyoruz
        _fileExtension = typeof(Tformat) == typeof(byte[]) ? "bin" : "json";
    }

    // Parametre olarak gelen dosya adına göre dinamik olarak tam dosya yolunu üreten yardımcı metot
    private string GetPath(string fileName)
    {
        return Path.Combine(_directory, string.Concat(fileName, ".", _fileExtension));
    }

    public void SaveData(Tdata data, string fileName)
    {
        string path = GetPath(fileName);
        try
        {
            // 1. Veriyi serileştir (Nesne -> JSON/Binary)
            Tformat serializedData = _dataSerializer.Serialize(data);

            // 2. Veriyi şifrele
            Tformat encryptedData = _dataEncryptor.Encrypt(serializedData);

            // 3. Şifrelenmiş veriyi diske yaz
            if (encryptedData is string jsonText)
            {
                File.WriteAllText(path, jsonText);
            }
            else if (encryptedData is byte[] binaryBytes)
            {
                File.WriteAllBytes(path, binaryBytes);
            }
            else
            {
                throw new NotSupportedException($"[FileDataHandler] {typeof(Tformat)} formatı dosya kaydı için desteklenmiyor!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Veri {path} adresine kaydedilemedi: {e.Message}");
        }
    }

    public Tdata LoadData(string fileName)
    {
        string path = GetPath(fileName);
        if (!FileExists(fileName))
        {
            return default;
        }

        try
        {
            Tformat fileContent;

            // 1. Dosyayı diskten oku
            if (typeof(Tformat) == typeof(string))
            {
                string text = File.ReadAllText(path);
                fileContent = (Tformat)(object)text;
            }
            else if (typeof(Tformat) == typeof(byte[]))
            {
                byte[] bytes = File.ReadAllBytes(path);
                fileContent = (Tformat)(object)bytes;
            }
            else
            {
                throw new NotSupportedException($"[FileDataHandler] {typeof(Tformat)} formatı diskten okunamaz!");
            }

            // 2. Okunan şifreli verinin şifresini çöz
            Tformat decryptedData = _dataEncryptor.Decrypt(fileContent);

            // 3. Şifresi çözülmüş temiz veriyi seriden çıkarıp nesneye dönüştür
            return _dataSerializer.Deserialize(decryptedData);
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Veri {path} adresinden yüklenemedi: {e.Message}");
            return default;
        }
    }

    public bool FileExists(string fileName)
    {
        string path = GetPath(fileName);
        return File.Exists(path);
    }

    public void DeleteFile(string fileName)
    {
        string path = GetPath(fileName);
        try
        {
            if (FileExists(fileName))
            {
                File.Delete(path);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Dosya silinemedi: {path}. Hata: {e.Message}");
        }
    }

    public List<string> GetAllFiles()
    {
        if (!Directory.Exists(_directory))
        {
            return new List<string>();
        }

        try
        {
            // Sadece bu sisteme ait olan uzantıdaki (*.json veya *.bin) dosyaları filtreler
            string searchPattern = $"*.{_fileExtension}";
            string[] files = Directory.GetFiles(_directory, searchPattern);
            return new List<string>(files);
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Dosya listesi alınamadı: {e.Message}");
            return new List<string>();
        }
    }

    public void DeleteAllFiles()
    {
        try
        {
            List<string> files = GetAllFiles();
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Tüm dosyalar silinemedi: {e.Message}");
        }
    }
}