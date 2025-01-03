using UnityEngine;
using UnityEngine.EventSystems;

public class SpinnerArrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Spinner parent;

    public bool Selected { get; private set; }

    private void Awake()
    {
        parent = GetComponentInParent<Spinner>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Selected = true;
        parent.SpinnerArrowSelected();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Selected = false;
        parent.SpinnerArrowDeselected();
    }
}