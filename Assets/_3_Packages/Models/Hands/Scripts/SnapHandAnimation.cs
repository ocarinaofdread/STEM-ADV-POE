using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hands.Scripts
{
    public class SnapHandAnimation : MonoBehaviour
    {
        [SerializeField] Animator handAnimator;
        [SerializeField] private InputActionReference triggerActionRef;
        [SerializeField] private InputActionReference gripActionRef;

        static readonly int TriggerHash = Animator.StringToHash("Trigger");
        static readonly int GripHash = Animator.StringToHash("Grip");

        private void OnEnable()
        {
            triggerActionRef.action.performed += TriggerHash_performed;
            triggerActionRef.action.canceled += TriggerHash_canceled;
            gripActionRef.action.performed += GripHash_performed;
            gripActionRef.action.canceled += GripHash_canceled;
        }

        private void TriggerHash_performed(InputAction.CallbackContext obj)
        {
            handAnimator.SetFloat(TriggerHash, 1);
        }
        
        private void TriggerHash_canceled(InputAction.CallbackContext obj)
        {
            handAnimator.SetFloat(TriggerHash, 0);
        }
        
        private void GripHash_performed(InputAction.CallbackContext obj)
        {
            handAnimator.SetFloat(GripHash, 1);
        }
        
        private void GripHash_canceled(InputAction.CallbackContext obj)
        {
            handAnimator.SetFloat(GripHash, 0);
        }

        private void OnDisable()
        {
            
        }
    }
}