using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUnityEventHandler : MonoBehaviour
{
    public Animator animator;
    public string boolName = "myBool";
    
    public void SetBool(bool value)
    {
        animator.SetBool(boolName, value);
    }
}
