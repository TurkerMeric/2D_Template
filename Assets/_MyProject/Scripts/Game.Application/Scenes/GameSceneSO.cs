using UnityEngine;


[CreateAssetMenu(fileName = "NewSceneConfig", menuName = "Game/Scene Configuration")]
public class GameSceneSO :ScriptableObject
{    
    [SerializeField] private string sceneId;    
    [SerializeField] private string sceneName;

#if UNITY_EDITOR
    
    [SerializeField] private UnityEditor.SceneAsset sceneAsset;

    private void OnValidate()
    {       
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
            
            if (string.IsNullOrEmpty(sceneId))
            {
                sceneId = name;
            }
        }
    }
#endif

    public string SceneId => sceneId;
    public string SceneName => sceneName;

}
