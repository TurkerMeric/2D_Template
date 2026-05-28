using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonFileHandler<T> : IDataHandler<T>
{
    private readonly IStringSerializer<T> _serializer;
    private readonly IDataEncryptor _encryptor;
    private readonly string _directory;
    private const string _fileExtension = "json";

    public JsonFileHandler(IStringSerializer<T> serializer, IDataEncryptor encryptor)
    {
        _serializer = serializer;
        _encryptor = encryptor;
        _directory = Application.persistentDataPath;
    }

    private string GetPath(string fileName)
    {
        return Path.Combine(_directory, string.Concat(fileName, ".", _fileExtension));
    }

    public void SaveData(T data, string fileName)
    {
        string path = GetPath(fileName);
        try
        {
            // 1. JSON'a çevir
            string json = _serializer.Serialize(data);

            // 2. Şifrele
            string encryptedJson = _encryptor.Encrypt(json);

            // 3. Diske düz metin olarak yaz
            File.WriteAllText(path, encryptedJson);
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonFileHandler] Veri {path} adresine kaydedilemedi: {e.Message}");
        }
    }

    public T LoadData(string fileName)
    {
        string path = GetPath(fileName);
        if (!FileExists(fileName))
        {
            return default;
        }

        try
        {
            // 1. Diskten oku
            string encryptedJson = File.ReadAllText(path);

            // 2. Şifreyi çöz
            string json = _encryptor.Decrypt(encryptedJson);

            // 3. Nesneye dönüştür
            return _serializer.Deserialize(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonFileHandler] Veri {path} adresinden yüklenemedi: {e.Message}");
            return default;
        }
    }

    public bool FileExists(string fileName) => File.Exists(GetPath(fileName));

    public void DeleteFile(string fileName)
    {
        string path = GetPath(fileName);
        if (FileExists(fileName))
        {
            File.Delete(path);
        }
    }

    public List<string> GetAllFiles()
    {
        if (!Directory.Exists(_directory)) return new List<string>();
        return new List<string>(Directory.GetFiles(_directory, $"*.{_fileExtension}"));
    }

    public void DeleteAllFiles()
    {
        foreach (var file in GetAllFiles())
        {
            if (File.Exists(file)) File.Delete(file);
        }
    }
}