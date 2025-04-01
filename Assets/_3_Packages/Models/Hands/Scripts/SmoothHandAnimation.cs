using UnityEngine;
using UnityEngine.InputSystem;

namespace Hands.Scripts
{
    public class SmoothHandAnimation : MonoBehaviour
    {
        [SerializeField] Animator handAnimator;
        [SerializeField] private InputActionReference triggerActionRef;
        [SerializeField] private InputActionReference gripActionRef;
    
        static readonly int TriggerHash = Animator.StringToHash("Trigger");
        static readonly int GripHash = Animator.StringToHash("Grip");

        void Update()
        {
            float triggerValue = triggerActionRef.action.ReadValue<float>();
            handAnimator.SetFloat(TriggerHash, triggerValue);
        
            float gripValue = gripActionRef.action.ReadValue<float>();
            handAnimator.SetFloat(GripHash, gripValue);
        }
    }
}
