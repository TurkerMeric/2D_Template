using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    // Kozmetik ve yazım düzeltmeleri yapılmıştır
    private const string _encryptionKey = "turkerMeric";
    private const string _fileName1 = "save_slot_1";
    private const string _fileName2 = "save_slot_2";

    private readonly GameData _newGameData1 = new()
    {
        Name = "Player_Slot_1_Game"
    };

    private readonly GameData _newGameData2 = new()
    {
        Name = "Player_Slot_2_Game"
    };

    private IDataHandler<GameData> _dataHandler;

    private void Awake()
    {
        // Artık yapıcı metotta dosya ismi (fileName) istemiyoruz.
        // Bu sayede aynı '_dataHandler' nesnesini dilediğimiz her dosya için ortak kullanabiliriz.
        _dataHandler = new FileDataHandler<GameData, string>(
            new NewtonsoftJsonSerializer<GameData>(),
            new XorEncryptor(_encryptionKey)
        );
    }

    private void Start()
    {
        _dataHandler.SaveData(_newGameData1, _fileName1);
        var data1 = _dataHandler.LoadData(_fileName1);

        if (data1 != null)
        {
            Debug.Log($"[Slot 1] {data1.Name} : veri başarıyla yüklendi.");
        }
                
        _dataHandler.SaveData(_newGameData2, _fileName2);
        var data2 = _dataHandler.LoadData(_fileName2);

        if (data2 != null)
        {
            Debug.Log($"[Slot 2] {data2.Name} : veri başarıyla yüklendi.");
        }
    }
}

public class GameData
{
    public string Name { get; set; }
}