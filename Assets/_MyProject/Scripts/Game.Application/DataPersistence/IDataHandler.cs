using System.Collections.Generic;

public interface IDataHandler<Tdata>
{
    void SaveData(Tdata data, string fileName);
    Tdata LoadData(string fileName);
    bool FileExists(string fileName);
    void DeleteFile(string fileName);
    void DeleteAllFiles();
    List<string> GetAllFiles();
}