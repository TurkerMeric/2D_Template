using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler<Tdata, Tformat> : IDataHandler<Tdata> 
{
    private readonly IDataSerializer<Tdata, Tformat> _dataSerializer;
    private readonly IDataEncryptor<Tformat> _dataEncryptor;
    private readonly string _path;
    private readonly string _directory;
    private readonly string _fileExtension;

    public FileDataHandler(IDataSerializer<Tdata, Tformat> dataSerializer, IDataEncryptor<Tformat> dataEncryptor, string fileName)
    {
        _dataSerializer = dataSerializer;
        _dataEncryptor = dataEncryptor;
        _directory = Application.persistentDataPath;
                
        _fileExtension = typeof(Tformat) == typeof(byte[]) ? "bin" : "json";
        _path = Path.Combine(_directory, string.Concat(fileName, ".", _fileExtension));
        Debug.Log("path = " + _path);
    }

    public void SaveData(Tdata data)
    {
        try
        {            
            Tformat serializedData = _dataSerializer.Serialize(data);
            Tformat encryptedData = _dataEncryptor.Encrypt(serializedData);
                        
            if (encryptedData is string jsonText)
            {
                File.WriteAllText(_path, jsonText);
            }
            else if (encryptedData is byte[] binaryBytes)
            {
                File.WriteAllBytes(_path, binaryBytes);
            }
            else
            {
                throw new NotSupportedException($"[FileDataHandler] {typeof(Tformat)} formatı dosya kaydı için desteklenmiyor!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Veri {_path} adresine kaydedilemedi: {e.Message}");
        }
    }

    public Tdata LoadData()
    {
        if (!FileExists())
        {
            return default;
        }

        try
        {
            Tformat fileContent;
            
            if (typeof(Tformat) == typeof(string))
            {
                string text = File.ReadAllText(_path);
                fileContent = (Tformat)(object)text;
            }
            else if (typeof(Tformat) == typeof(byte[]))
            {
                byte[] bytes = File.ReadAllBytes(_path);
                fileContent = (Tformat)(object)bytes;
            }
            else
            {
                throw new NotSupportedException($"[FileDataHandler] {typeof(Tformat)} formatı diskten okunamaz!");
            }

            
            Tformat decryptedData = _dataEncryptor.Decrypt(fileContent);

            return _dataSerializer.Deserialize(decryptedData);
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Veri {_path} adresinden yüklenemedi: {e.Message}");
            return default;
        }
    }

    public bool FileExists()
    {
        return File.Exists(_path);
    }

    public void DeleteFile()
    {
        try
        {
            if (FileExists())
            {
                File.Delete(_path);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[FileDataHandler] Dosya silinemedi: {_path}. Hata: {e.Message}");
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