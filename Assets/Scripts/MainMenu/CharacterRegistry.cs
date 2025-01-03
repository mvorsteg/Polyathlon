using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/CharacterRegistry")]
public class CharacterRegistry : ScriptableObject
{
    public string displayName;
    public string subtitle;
    public string info;
    public Sprite icon;
    //public GameObject model;    
    public GameObject playerObj;
    public GameObject npcObj;
    public GameObject previewObj;

}