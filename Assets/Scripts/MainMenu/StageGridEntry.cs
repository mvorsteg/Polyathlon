using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageGridEntry : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private Image characterImage;
    private Selectable button;
    public Selectable Button { get => button; }

    private void Awake()
    {
        button = GetComponent<Selectable>();
    }

    public void Initialize(string name, Sprite image)
    {
        nameText.text = name;
        characterImage.sprite = image;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        // if (true) // TODO only if P1 has mouse
        // {
        //     if (mouseSelector.selectedCharacter != this && !mouseSelector.Locked)
        //     {
        //         mouseSelector.selectedCharacter.RemoveSelector(mouseSelector);
        //         mouseSelector.selectedCharacter = this;
        //         AddSelector(mouseSelector);
        //     }
        // }
    }
}