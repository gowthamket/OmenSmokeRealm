using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SMBFunctions
{
    public static float GetNormalizedTime(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime > 1 ? 1 : stateInfo.normalizedTime;
    }
    public static int GetCurrentFrame(int totalFrames, float normalizedTime)
    {
        return Mathf.RoundToInt(totalFrames * normalizedTime);
    }
    public static bool HasParameter(Animator animator, string parameterName)
    {
        if (string.IsNullOrEmpty(parameterName) || string.IsNullOrWhiteSpace(parameterName))
            return false;

        foreach (var parameter in animator.parameters)
            if (parameter.name == parameterName)
                return true;

        return false;
    }


    /// <summary>
    /// Should only be used in onEnter, or when you know that there is a next clip
    /// Keep and eye on this
    /// </summary>   
    public static int GetTotalFrames(Animator animator, int layerIndex)
    {
        AnimatorClipInfo[] clipInfos = animator.GetNextAnimatorClipInfo(layerIndex);
        if (clipInfos.Length == 0)
            clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);

        AnimationClip clip = clipInfos[0].clip;
        return Mathf.RoundToInt(clip.length * clip.frameRate);
    }

}
