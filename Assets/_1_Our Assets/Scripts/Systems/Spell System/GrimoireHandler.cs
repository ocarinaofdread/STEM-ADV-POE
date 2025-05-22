using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GrimoireHandler : MonoBehaviour
{
    // Dominant Hand: Controls
    // Non-Dominant Hand: Spells
    [SerializeField] private TextMeshProUGUI leftPageTMPro;
    [SerializeField] private TextMeshProUGUI rightPageTMPro;
    
    [TextArea(1, 3)] [SerializeField] private string controlsText;

    // <Name, Description>
    private Dictionary<string, string> _spellDictionary;
    private GameManager _gameManager;
    private DominantHand _currentDominantHand;
    
    private TextMeshProUGUI _currentSpellPage;
    private TextMeshProUGUI _currentControlsPage;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _spellDictionary = _gameManager.GetSpellDictionary();
        
        _currentDominantHand = _gameManager.GetDominantHand();
        SetPages(_currentDominantHand);

        var firstSpellName = _spellDictionary.Keys.ToList()[0];
        UpdateGrimoirePages(firstSpellName ,1);
        UpdateGrimoirePages("", 2);
    }
    
    private void Update()
    {
        if (_currentDominantHand == _gameManager.GetDominantHand()) return;
        
        ChangeDominantHand(_gameManager.GetDominantHand(), true);
    }
    
    public void UpdateGrimoirePages(string newSpellName, int changeType)
    {
        if (_spellDictionary == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
            _spellDictionary = _gameManager.GetSpellDictionary();
        }

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
        // if (_currentDominantHand == dominantHand) return;
        
        _currentDominantHand = dominantHand;
        SetPages(_currentDominantHand);
            
        if (swapAfter) { UpdateGrimoirePages(null, 3); }
    }

    private void SetPages(DominantHand dominantHand)
    {
        // Remember: Grimoire is held in NON-DOMINANT HAND!
        if (dominantHand == DominantHand.LeftHanded)
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
}
