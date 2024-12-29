using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spinner : Selectable
{
    [SerializeField]
    private Image leftArrow, rightArrow;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private List<string> values;
    private int index = 0;

    public bool wrapAround = false;
    public bool automaticSize = true;

    public UnityEvent onValueChanged;

    public string Value { get => values[index]; }

    protected override void Awake()
    {
        if (automaticSize)
        {
            float maxWidth = text.rectTransform.sizeDelta.x;
            
            ContentSizeFitter fitter = text.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            foreach (string val in values)
            {
                text.text = val;
                text.ForceMeshUpdate();
                Canvas.ForceUpdateCanvases();
                maxWidth = Mathf.Max(maxWidth, text.rectTransform.sizeDelta.x);
            }

            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            text.rectTransform.sizeDelta = new Vector2(maxWidth, text.rectTransform.sizeDelta.y);
            Destroy(fitter);
        }

        if (values.Count > 0)
        {
            text.text = values[0];
        }
    }

    public void ClearValues()
    {
        values.Clear();
    }

    public void AddValue(string value)
    {
        values.Add(value);
        if (values.Count == 1)
        {
            text.text = values[0];
        }
    }

    public void Navigate(bool isRight)
    {
        if (isRight)
        {
            if (wrapAround || index < values.Count - 1)
            {
                index++;
            }
            if (index >= values.Count)
            {
                index = 0;
            }
        }
        else
        {
            if (wrapAround || index > 0)
            {
                index --;
            }
            if (index < 0)
            {
                index = values.Count - 1;
            }
        }

        text.text = values[index];

        if (onValueChanged != null)
        {
            onValueChanged.Invoke();
        }
    }

    public void SkipToValue(string value)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] == value)
            {
                index = i;
                text.text = values[index];
                onValueChanged.Invoke();
                break;
            }
        }
    }

    private void LimitSelectionArrows()
    {
        if (!wrapAround)
        {
            if (index == 0)
            {
                // disable left arrow
            }
            else if (index == values.Count - 1)
            {
                // disable right arrow
            }
        }
    }
}