using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DominantHand
{
    RightHanded,
    LeftHanded
}

public class GameManager : MonoBehaviour
{
    // Spell Handler
    [SerializeField] private GameObject[] spellPrefabList;
    [SerializeField] private DominantHand selectedDominantHand;
    
    private string[] _spellNames;
    private string[] _spellDescriptions;
    private Dictionary<string, string> _spellDictionary = new();

    private void Awake()
    {
        InitializeSpellListsAndDictionary();
    }

    // Spell initialization & management
    // ReSharper disable Unity.PerformanceAnalysis
    private void InitializeSpellListsAndDictionary()
    {
        // Initialize Lists
        _spellNames = new string[spellPrefabList.Length];
        _spellDescriptions = new string[spellPrefabList.Length];
        
        for (int i = 0; i < spellPrefabList.Length; i++)
        {
            _spellNames[i] = spellPrefabList[i].GetComponentInChildren<Spell>().GetName();
            _spellDescriptions[i] = spellPrefabList[i].GetComponentInChildren<Spell>().GetDescription();
        }

        // Initialize Dictionary
        if (_spellNames.Length != _spellDescriptions.Length)
        {
            Debug.Log("Spell dictionary could not be initialized: SpellNames " +
                      "and SpellDescriptions arrays are not of equal length.");
            return;
        }

        _spellDictionary = new Dictionary<string, string>();
        
        for (int i = 0; i < _spellNames.Length; i++)
        {
            _spellDictionary.Add(_spellNames[i], _spellDescriptions[i]);
        }
    }
    
    // Get methods
    public GameObject[] GetSpellPrefabList() => spellPrefabList;
    public Dictionary<string, string> GetSpellDictionary()
    {
        if (_spellDictionary == null)
        {
            InitializeSpellListsAndDictionary();
        }
        return _spellDictionary;
    }
    public DominantHand GetDominantHand() => selectedDominantHand;
    
    // Set methods
    public void SetDominantHand(DominantHand newHand){
        // ReSharper disable once RedundantCheckBeforeAssignment
        if (newHand == selectedDominantHand) return;
        
        selectedDominantHand = newHand;
    }
}
