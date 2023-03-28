using UnityEngine;

[CreateAssetMenu(fileName = "PrefabDB", menuName = "ScriptableObject/Prefab Database", order = 1)]
public class PrefabDatabase : ScriptableObject
{
    public GameObject[] prefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] orbPrefabs;
}
