using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneRegistry", menuName = "Game/Scene Registry")]
public class SceneRegistrySO : ScriptableObject
{
    [SerializeField] private List<GameSceneSO> registeredScenes;

    // String ID'yi verip gerçek ScriptableObject'i güvenle çekiyoruz
    public GameSceneSO GetSceneById(string id)
    {
        var found = registeredScenes.Find(s => s.SceneId == id);
        if (found == null)
        {
            Debug.LogError($"[SceneRegistry] '{id}' kimliğine sahip sahne bulunamadı!");
        }
        return found;
    }
}