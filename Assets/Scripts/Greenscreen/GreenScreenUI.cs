using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GreenScreenUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI modeButtonText, targetButtonText;
    [SerializeField]
    private Slider animationFrameSlider;
    [SerializeField]
    private TMP_Dropdown animationList;
    [SerializeField]
    private GreenScreenAnimationController animationController;

    private List<AnimationClip> animationLookup;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        animationLookup = new List<AnimationClip>();
    }

    public void SetMode(ControlMode newMode)
    {
        modeButtonText.text = string.Format("Controls: {0}", newMode);
    }

    public void SetTargetMode(TargetMode newMode)
    {
        targetButtonText.text = string.Format("Target: {0}", newMode);
    }

    public void LoadAnimations(IEnumerable<AnimationClip> clips)
    {
        animationList.ClearOptions();
        animationLookup.Clear();

        List<string> options = new List<string>();
        foreach (AnimationClip clip in clips)
        {
            options.Add(clip.name);
            animationLookup.Add(clip);
        }
        animationList.AddOptions(options);

        LoadAnimation(0);
    }

    public void LoadAnimation(int idx)
    {
        AnimationClip clip = animationLookup[idx];
        animationFrameSlider.maxValue = clip.frameRate * clip.length;
        animationController.LoadClip(clip);
        animationFrameSlider.value = 0;
    }


    public void OnSliderValueChanged()
    {
        int frame = (int)animationFrameSlider.value;
        animationController.SetFrame(frame);
    }

    public void Scrub(bool pos)
    {
        if (pos)
        {
            animationFrameSlider.value += 1;
        }
        else
        {
            animationFrameSlider.value -= 1;
        }
    }
}