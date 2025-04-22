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
    [SerializeField] private TextMeshProUGUI LeftPageTMPro;
    [SerializeField] private TextMeshProUGUI RightPageTMPro;
    
    [SerializeField] private DominantHand selectedDominantHand;
    [SerializeField] private string[] spellNames;
    [TextArea(3, 5)]
    [SerializeField] string[] spellDescriptions;

    // <Name, Description>
    private Dictionary<string, string> _spellDictionary;
    private int _currentSpellValue;
    
    void Start()
    {
        
        if (CreateSpellDictionary())
        {
            
        }
    }

    public void ChangeSpellDescription(string spellName)
    {
        
    }

    public bool CreateSpellDictionary()
    {
        if (spellNames.Length != spellDescriptions.Length)
        {
            Debug.Log("Spell dictionary could not be initialized: SpellNames " +
                      "and SpellDescriptions arrays are not of equal length.");
            return false;
        }
        
        for (int i = 0; i < spellNames.Length; i++)
        {
            _spellDictionary.Add(spellNames[i], spellDescriptions[i]);
        }

        return true;
    }
    
    
    public void SetDominantHand(DominantHand dominantHand) => selectedDominantHand = dominantHand;
    public DominantHand GetDominantHand() => selectedDominantHand;
}
