using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PhotoModeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject controlsPanel;
    [SerializeField]
    private TextMeshProUGUI lookText, moveText, upText, downText, photoText, hideText, exitText;

    [SerializeField]
    private Image topShutter, bottomShutter;
    [SerializeField]
    private float shutterCloseTime, shutterFadeTime;
    [SerializeField]
    private AudioClip shutterClip;
    private Color transparent, opaque;
    private Vector2 maxScreenSize;
    private AudioSource audioSource;
    private bool displayUI = true;


    private void Awake()
    {
        CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
        if (canvasScaler != null)
        {
            maxScreenSize = canvasScaler.referenceResolution;
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("PhotoModeUI does not have an AudioSource");
        }

        transparent = new Color(1f, 1f, 1f, 0f);
        opaque = new Color(1f, 1f, 1f, 1f);
    }

    private void OnEnable()
    {
        displayUI = true;
        controlsPanel.SetActive(true);

        topShutter.color = transparent;
        bottomShutter.color = transparent;        
        topShutter.rectTransform.sizeDelta = new Vector2(topShutter.rectTransform.sizeDelta.x, 0f);
        bottomShutter.rectTransform.sizeDelta = new Vector2(bottomShutter.rectTransform.sizeDelta.x, 0f);
    }

    public void UpdateControlsText(PlayerInput playerInput)
    {
        InputActionMap actionMap = playerInput.actions.FindActionMap("PhotoMode");
        string lookButton =     GamepadUtility.GetButtonFromInput(actionMap.FindAction("Look"), playerInput.currentControlScheme);
        string moveButton =     GamepadUtility.GetButtonFromInput(actionMap.FindAction("Movement"), playerInput.currentControlScheme);
        string upButton =       GamepadUtility.GetButtonFromInput(actionMap.FindAction("Up"), playerInput.currentControlScheme);
        string downButton =     GamepadUtility.GetButtonFromInput(actionMap.FindAction("Down"), playerInput.currentControlScheme);
        string photoButton =    GamepadUtility.GetButtonFromInput(actionMap.FindAction("TakePhoto"), playerInput.currentControlScheme);
        string hidebutton =     GamepadUtility.GetButtonFromInput(actionMap.FindAction("HideUI"), playerInput.currentControlScheme);
        string exitButton =     GamepadUtility.GetButtonFromInput(actionMap.FindAction("Pause"), playerInput.currentControlScheme);

        lookText.text =     string.Format("[{0}] Look", lookButton);
        moveText.text =     string.Format("[{0}] Move", moveButton);
        upText.text =       string.Format("[{0}] Up", upButton);
        downText.text =     string.Format("[{0}] Down", downButton);
        photoText.text =    string.Format("[{0}] Take Photo", photoButton);
        hideText.text =     string.Format("[{0}] Hide UI", hidebutton);
        exitText.text =     string.Format("[{0}] Exit Photo Mode", exitButton);
    }

    public void ToggleHideUI()
    {
        displayUI = !displayUI;
        controlsPanel.SetActive(displayUI);
    }

    public void TakeSnapshot()
    {
        StartCoroutine(ShuttersCoroutine());
    }

    private IEnumerator ShuttersCoroutine()
    {
        topShutter.color = opaque;
        bottomShutter.color = opaque;

        float elapsedTime = 0f;
        while (elapsedTime < shutterCloseTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float height = Mathf.Lerp(0f, maxScreenSize.y / 2f, elapsedTime / shutterCloseTime);
            topShutter.rectTransform.sizeDelta = new Vector2(topShutter.rectTransform.sizeDelta.x, height);
            bottomShutter.rectTransform.sizeDelta = new Vector2(bottomShutter.rectTransform.sizeDelta.x, height);
            yield return null;
        }

        topShutter.rectTransform.sizeDelta = new Vector2(topShutter.rectTransform.sizeDelta.x, maxScreenSize.y);
        bottomShutter.rectTransform.sizeDelta = new Vector2(bottomShutter.rectTransform.sizeDelta.x, maxScreenSize.y);

        audioSource.clip = shutterClip;
        audioSource.Play();

        elapsedTime = 0f;
        while (elapsedTime < shutterFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Color col = Color.Lerp(opaque, transparent, elapsedTime / shutterFadeTime);
            topShutter.color = col;
            bottomShutter.color = col;
            yield return null;
        }
        topShutter.color = transparent;
        bottomShutter.color = transparent;
    }
}