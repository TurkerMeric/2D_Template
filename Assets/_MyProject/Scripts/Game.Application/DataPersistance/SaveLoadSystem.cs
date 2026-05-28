using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    private const string _encriptionKey = "turkerMeric";
    private const string _fileName = "save";
    private readonly GameData _newGameData = new()
    {
        Name = "NewGame"
    };

    private IDataHandler<GameData> _dataHandler;

    private void Awake()
    {
        _dataHandler = new FileDataHandler<GameData, string>
        (new NewtonsoftJsonSerializer<GameData>()
        , new XorEncryptor(_encriptionKey)
        , _fileName);
    }
    private void Start()
    {
        _dataHandler.SaveData(_newGameData);
        var data = _dataHandler.LoadData();
        Debug.Log($"{data.Name} : data name loaded success.");
    }
}

public class GameData
{
    public string Name { get; set; }
}