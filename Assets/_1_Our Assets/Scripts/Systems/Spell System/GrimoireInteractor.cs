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

    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {   
        // Remember: You hold the grimoire in your NON-DOMINANT HAND
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            attachTransform = leftHandAttachTransform;
            rightHandSpellHandler.StartHoldingGrimoire();
            _gameManager.SetDominantHand(DominantHand.RightHanded);
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            attachTransform = rightHandAttachTransform;
            leftHandSpellHandler.StartHoldingGrimoire();
            _gameManager.SetDominantHand(DominantHand.LeftHanded);
        }

        base.OnSelectEntering(args);
    }
}
