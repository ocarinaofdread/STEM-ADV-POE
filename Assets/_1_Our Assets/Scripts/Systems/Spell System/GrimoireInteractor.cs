// Credits to rsx83 on Unity Discussions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrimoireInteractor : XRGrabInteractable
{
    public Transform leftHandAttachTransform;
    public Transform rightHandAttachTransform;

    [SerializeField] SpellHandler leftHandSpellHandler;
    [SerializeField] SpellHandler rightHandSpellHandler;

    private GrimoireHandler handler;
    
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        handler = GetComponent<GrimoireHandler>();
        
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            attachTransform = leftHandAttachTransform;
            rightHandSpellHandler.StartHoldingGrimoire();
            handler.ChangeDominantHand(DominantHand.LeftHanded, true);
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            attachTransform = rightHandAttachTransform;
            leftHandSpellHandler.StartHoldingGrimoire();
            handler.ChangeDominantHand(DominantHand.RightHanded, true);
        }

        base.OnSelectEntering(args);
    }
}
