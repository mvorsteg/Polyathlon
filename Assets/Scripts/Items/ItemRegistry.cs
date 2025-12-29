using UnityEngine;

[CreateAssetMenu(fileName = "ItemRegistry", menuName = "ScriptableObjects/ItemRegistry")]
public class ItemRegistry : ScriptableObject
{
    public string displayName;
    public string description;
    public Sprite icon;
    public Item itemPrefab;

}