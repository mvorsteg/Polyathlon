using TMPro;
using UnityEngine;

public class AllReadyOverlay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    public void SetActive(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public void SetControlScheme(ControlScheme scheme)
    {
        string buttonToPress = "[button]";
        if (scheme == ControlScheme.Keyboard)
        {
            buttonToPress = "Enter";
        }
        else if (scheme == ControlScheme.Gamepad)
        {
            buttonToPress = "A";
        }
        text.text = string.Format("Everyone is ready!\nPress {0} to continue", buttonToPress);
    }
}