using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DominantHand
{
    RightHanded,
    LeftHanded
}

public class GrimoireHandler : MonoBehaviour
{
    // Dominant Hand: Controls
    // Non-Dominant Hand: Spells
    [SerializeField] private TextMeshProUGUI leftPageTMPro;
    [SerializeField] private TextMeshProUGUI rightPageTMPro;
    
    [SerializeField] private DominantHand selectedDominantHand;
    [SerializeField] private string[] spellNames;
    [TextArea(1, 3)] [SerializeField] string[] spellDescriptions;
    [TextArea(1, 3)] [SerializeField] private string controlsText;

    // <Name, Description>
    private Dictionary<string, string> _spellDictionary;

    private void Start()
    {
        ChangeControlsPage();
    }
    
    
    public void ChangeSpellPage(string newSpellName)
    {
        if (_spellDictionary == null){ CreateSpellDictionary(newSpellName); }
        else
        {
            var newSpellDescription = _spellDictionary[newSpellName];

            if (selectedDominantHand == DominantHand.RightHanded)
            {
                leftPageTMPro.text = newSpellDescription;
            }
            else
            {
                rightPageTMPro.text = newSpellDescription;
            }
        }
    }

    private void CreateSpellDictionary(string firstSpell)
    {
        if (spellNames.Length != spellDescriptions.Length)
        {
            Debug.Log("Spell dictionary could not be initialized: SpellNames " +
                      "and SpellDescriptions arrays are not of equal length.");
            return;
        }
        
        Debug.Log("Initializing spell dictionary...");
        for (int i = 0; i < spellNames.Length; i++)
        {
            Debug.Log(spellNames[i] + " with description " + spellDescriptions[i] +
                      " is about to be added.");
            Debug.Log(_spellDictionary + " exists.");
            _spellDictionary.Add(spellNames[i], spellDescriptions[i]);
            Debug.Log("Spell Dictionary slot " + i + " has been added.");
        }
        
        Debug.Log("Spell dictionary has been initialized.");
        ChangeSpellPage(firstSpell);
    }

    private void ChangeControlsPage()
    {
        if (selectedDominantHand == DominantHand.RightHanded) 
        { rightPageTMPro.text = controlsText; }
        else 
        { leftPageTMPro.text = controlsText; }
    }
    
    
    public void SetDominantHand(DominantHand dominantHand) => selectedDominantHand = dominantHand;
    public DominantHand GetDominantHand() => selectedDominantHand;
}
