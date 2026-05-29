using UnityEngine.SceneManagement;

public class SceneStateManager
{
    private readonly SceneRegistrySO _sceneRegistry;
    private GameSceneSO _currentScene;

    public SceneStateManager(SceneRegistrySO sceneRegistry)
    {
        _sceneRegistry = sceneRegistry;
    }

    // 1. Oyuncu sahne değiştirmek istediğinde doğrudan SO nesnesini verir:
    public void ChangeScene(GameSceneSO newScene)
    {
        _currentScene = newScene;

        // Unity'nin sahne yükleme sistemine SO'nun içindeki otomatik eşitlenmiş ismi veririz
        SceneManager.LoadScene(_currentScene.SceneName);
    }

    // 2. SaveLoadSystem diskten verileri yüklediğinde bu metot tetiklenir:
    public void OnDataLoaded(GameData data)
    {
        // Kayıt dosyasındaki string ID'yi gerçek akıllı sahne nesnesine dönüştürürüz
        _currentScene = _sceneRegistry.GetSceneById(data.ActiveSceneId);

        // Ve sahneyi yükleriz
        SceneManager.LoadScene(_currentScene.SceneName);
    }

    // 3. Oyun kaydedileceği zaman mevcut durum veriye yazılır:
    public void OnDataSaved(GameData data)
    {
        if (_currentScene != null)
        {
            data.ActiveSceneId = _currentScene.SceneId;
        }
    }
}