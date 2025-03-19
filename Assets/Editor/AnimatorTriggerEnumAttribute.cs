using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AnimatorTriggerEnumAttribute : PropertyAttribute
{
    public Animator animator;

    public AnimatorTriggerEnumAttribute(Animator animator)
    {
        this.animator = animator;
    }
}