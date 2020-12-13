using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterList", order = 1)]
public class CharacterList : ScriptableObject
{
    [SerializeField]
    private Character[] characters;

    public Character[] GetCharacters()
    {
        return characters;
    }

}
