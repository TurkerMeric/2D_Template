using System.Collections.Generic;

public interface IDataHandler<Tdata>
{
    void SaveData(Tdata data);
    Tdata LoadData();
    bool FileExists(); 
    void DeleteFile();
    void DeleteAllFiles(); 
    List<string> GetAllFiles();
}