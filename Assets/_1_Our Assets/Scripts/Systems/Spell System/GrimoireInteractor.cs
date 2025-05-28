// Credits to rsx83 on Unity Discussions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrimoireInteractor : XRGrabInteractable
{
    public Transform leftHandAttachTransform;
    public Transform rightHandAttachTransform;

    private SpellHandler _leftHandSpellHandler;
    private SpellHandler _rightHandSpellHandler;
    private GameManager _gameManager;
    
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {   
        // Remember: You hold the grimoire in your NON-DOMINANT HAND
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            _rightHandSpellHandler ??= GameObject.FindGameObjectWithTag("RightHand").GetComponent<SpellHandler>();
            attachTransform = leftHandAttachTransform;
            _rightHandSpellHandler.StartHoldingGrimoire();
            //_gameManager.SetDominantHand(DominantHand.RightHanded);
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            _leftHandSpellHandler ??= GameObject.FindGameObjectWithTag("LeftHand").GetComponent<SpellHandler>();
            attachTransform = rightHandAttachTransform;
            _leftHandSpellHandler.StartHoldingGrimoire();
            //_gameManager.SetDominantHand(DominantHand.LeftHanded);
        }

        base.OnSelectEntering(args);
    }
    
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {   
        // Remember: You hold the grimoire in your NON-DOMINANT HAND
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            _rightHandSpellHandler ??= GameObject.FindGameObjectWithTag("RightHand").GetComponent<SpellHandler>();
            //attachTransform = leftHandAttachTransform;
            _rightHandSpellHandler.StopHoldingGrimoire();
            //_gameManager.SetDominantHand(DominantHand.RightHanded);
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            _leftHandSpellHandler ??= GameObject.FindGameObjectWithTag("LeftHand").GetComponent<SpellHandler>();
            //attachTransform = rightHandAttachTransform;
            _leftHandSpellHandler.StopHoldingGrimoire();
            //_gameManager.SetDominantHand(DominantHand.LeftHanded);
        }

        base.OnSelectExiting(args);
    }
}
