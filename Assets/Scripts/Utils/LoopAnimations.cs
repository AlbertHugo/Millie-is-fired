using UnityEditor;
using UnityEngine;

public class LoopAnimations : MonoBehaviour
{
    public AnimationClip animationClip;
    private AnimationClipSettings aniSettings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aniSettings = AnimationUtility.GetAnimationClipSettings(animationClip);
        aniSettings.loopTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
