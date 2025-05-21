using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_Electricity : Spell
{
    //[SerializeField] private Animator animator;
    
    protected override void RunCastAction()
    {
        
    }

    public override void End()
    {
        Destroy(gameObject);
    }
}
