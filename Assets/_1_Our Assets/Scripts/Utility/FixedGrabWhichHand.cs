// Credits to rsx83 on Unity Discussions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FixedGrabWhichHand : XRGrabInteractable
{
    public Transform leftHandAttachTransform;
    public Transform rightHandAttachTransform;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            attachTransform = leftHandAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            attachTransform = rightHandAttachTransform;
        }

        base.OnSelectEntering(args);
    }
}
