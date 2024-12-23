using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterGridEntry : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private Image characterImage;
    private List<Transform> targets;
    [SerializeField]
    private Transform targetParent;
    private Selectable button;
    private List<PlayerSelector> currentSelectors; // TODO change to dict w/ max cap???
    private PlayerSelector mouseSelector;   // special selector for the player who is using a mouse

    public Selectable Button { get => button; }
    public int PlayerCount { get => currentSelectors.Count; }

    private void Awake()
    {
        button = GetComponent<Selectable>();   

        currentSelectors = new List<PlayerSelector>();
        targets = new List<Transform>();
        foreach (Transform target in targetParent)
        {
            targets.Add(target);
        }
    }

    private void Start()
    {
        foreach (Transform target in targetParent)
        {
            target.gameObject.SetActive(false);
        }
    }

    public void Initialize(string name, Sprite image)
    {
        nameText.text = name;
        characterImage.sprite = image;
    }

    public void AddSelector(PlayerSelector selector)
    {
        currentSelectors.Add(selector);
        currentSelectors.Sort((a, b) => a.PlayerIndex.CompareTo(b.PlayerIndex));
        Redraw();
    }

    public void RemoveSelector(PlayerSelector selector)
    {
        currentSelectors.Remove(selector);
        Redraw();
    }

    public void SetMouseSelector(PlayerSelector selector)
    {
        mouseSelector = selector;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (mouseSelector != null)
        {
            if (mouseSelector.selectedCharacter != this && !mouseSelector.Locked)
            {
                mouseSelector.selectedCharacter.RemoveSelector(mouseSelector);
                mouseSelector.selectedCharacter = this;
                AddSelector(mouseSelector);
            }
        }
    }

    private void Redraw()
    {
        foreach (Transform target in targetParent)
        {
            target.gameObject.SetActive(false);
        }

        for (int i = 0; i < currentSelectors.Count; i++)
        {
            targets[i].gameObject.SetActive(true);
            currentSelectors[i].MoveToTarget(targets[i]);
        }
    }

}