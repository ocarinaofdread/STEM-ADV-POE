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
    [TextArea(1, 3)] [SerializeField] private string controlsText;

    // <Name, Description>
    private Dictionary<string, string> _spellDictionary;
    private GameManager _gameManager;

    private TextMeshProUGUI _currentSpellPage;
    private TextMeshProUGUI _currentControlsPage;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _spellDictionary = _gameManager.GetSpellDictionary();
        UpdateGrimoirePages(null, 2);
    }
    
    
    public void UpdateGrimoirePages(string newSpellName, int changeType)
    {
        _spellDictionary ??= _gameManager.GetSpellDictionary();

        switch (changeType)
        {
            // 1 = Change Spell Page
            case 1:
                var newSpellDescription = _spellDictionary[newSpellName];
                _currentSpellPage.text = newSpellDescription;
                break;
            // 2 = Change Controls Page
            case 2:
                _currentControlsPage.text = controlsText;
                break;
            // 3 = Swap Pages
            case 3:
                _currentSpellPage.text = _currentControlsPage.text;
                _currentControlsPage.text = controlsText;
                break;
        }
    }


    public void ChangeDominantHand(DominantHand dominantHand, bool swapAfter)
    {
        if (selectedDominantHand != dominantHand)
        {
            selectedDominantHand = dominantHand;
            if (selectedDominantHand == DominantHand.RightHanded)
            {
                _currentSpellPage = leftPageTMPro;
                _currentControlsPage = rightPageTMPro;
            }
            else
            {
                _currentSpellPage = rightPageTMPro;
                _currentControlsPage = leftPageTMPro;
            }
            
            if (swapAfter) { UpdateGrimoirePages(null, 3); }
        }
    }

    public DominantHand GetDominantHand() => selectedDominantHand;
}
