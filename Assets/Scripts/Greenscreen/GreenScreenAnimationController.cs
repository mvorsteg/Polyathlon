using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GreenScreenAnimationController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private GreenScreenUI ui;

    private Animator animator;
    [SerializeField]
    private RuntimeAnimatorController poseController;
    private AnimationClip selectedClip;

    private List<AnimationClip> allAnimations;

    private void Awake()
    {
        allAnimations = new List<AnimationClip>();
    }

    private void Start()
    {
        if (target.TryGetComponent<Animator>(out Animator animator))
        {
            LoadAnimator(animator);
        }    
    }

    public void LoadAnimator(Animator animator)
    {
        this.animator = animator;

        // foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        // {
        //     allAnimations.Add(clip);
        // }
        AnimationClip firstAnimation = animator.runtimeAnimatorController.animationClips[0];
        ui.LoadAnimations(animator.runtimeAnimatorController.animationClips);

        animator.runtimeAnimatorController = poseController;
        animator.speed = 0f;
        
        LoadClip(firstAnimation);
        
    }    

    public void LoadClip(AnimationClip clip)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        List<KeyValuePair<AnimationClip, AnimationClip>> anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (AnimationClip a in aoc.animationClips)
        {
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, clip));
        }
        aoc.ApplyOverrides(anims);
        animator.runtimeAnimatorController = aoc;

        selectedClip = clip;
        animator.Play(0, 0, 0);
    }

    public void SetFrame(int frame)
    {
        float lastFrame = selectedClip.length * selectedClip.frameRate;
        float currTime = frame / lastFrame;
        animator.Play(0, 0, currTime);
    }

}