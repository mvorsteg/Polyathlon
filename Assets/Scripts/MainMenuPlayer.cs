using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuPlayer : MonoBehaviour
{
    public CharacterList characterList;
    private MainMenuManager manager;
    private Character[] characters;
    private int characterIndex;
    private GameObject currentCharPreview;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectsOfType<MainMenuManager>()[0];
        characters = characterList.GetCharacters();
        currentCharPreview = Instantiate(characters[characterIndex].previewObj, this.transform);
        manager.JoinPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNavigate(InputValue value)
    {
        Vector2 vecVal = value.Get<Vector2>();
        if (vecVal.x > 0.5)
        {
            CycleCharacter(true);
        }
        else if (vecVal.x < -0.5)
        {
            CycleCharacter(false);
        }
    }

    private void CycleCharacter(bool forward)
    {
        // Determine how to move our index
        if (forward)
        {
            if (characterIndex + 1 < characters.Length)
                characterIndex++;
            else
                characterIndex = 0;
        }
        else
        {
            if (characterIndex - 1 >= 0)
                characterIndex--;
            else
                characterIndex = characters.Length - 1;
        }
        // remove the old character preview and instantiate a new one
        Destroy(currentCharPreview);
        currentCharPreview = Instantiate(characters[characterIndex].previewObj, this.transform);
    }
}
