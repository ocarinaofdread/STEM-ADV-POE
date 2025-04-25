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

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _spellDictionary = _gameManager.GetSpellDictionary();
        SetPages(selectedDominantHand);
        UpdateGrimoirePages("", 2);
    }
    
    
    public void UpdateGrimoirePages(string newSpellName, int changeType)
    {
        _spellDictionary ??= _gameManager.GetSpellDictionary();
        if (_spellDictionary == null) { Debug.Log("Dictionary is null!");}
        if (_gameManager == null) { Debug.Log("Manager is null!");}

        switch (changeType)
        {
            // 1 = Change Spell Page
            case 1:
                if (_spellDictionary[newSpellName] == null) { Debug.Log("Dictionary[SpellName] is null!");}
                var newSpellDescription = _spellDictionary[newSpellName];
                if (_currentSpellPage == null) { Debug.Log("Spell page is null!");}
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
            SetPages(selectedDominantHand);
            
            if (swapAfter) { UpdateGrimoirePages(null, 3); }
        }
    }

    private void SetPages(DominantHand dominantHand)
    {
        if (dominantHand == DominantHand.RightHanded)
        {
            _currentSpellPage = leftPageTMPro;
            _currentControlsPage = rightPageTMPro;
        }
        else
        {
            _currentSpellPage = rightPageTMPro;
            _currentControlsPage = leftPageTMPro;
        }
    }

    public DominantHand GetDominantHand() => selectedDominantHand;
}
