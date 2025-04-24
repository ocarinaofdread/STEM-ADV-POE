using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Spell Handler
    [SerializeField] private GameObject[] spellPrefabList;
    
    private string[] _spellNames;
    private string[] _spellDescriptions;
    private Dictionary<string, string> _spellDictionary = new();

    void Start()
    {
        InitializeSpellLists();
    }

    // Spell initialization & management
    private void InitializeSpellLists()
    {
        _spellNames = new string[spellPrefabList.Length];
        _spellDescriptions = new string[spellPrefabList.Length];
        
        for (int i = 0; i < spellPrefabList.Length; i++)
        {
            _spellNames[i] = spellPrefabList[i].GetComponent<Spell>().GetName();
            _spellDescriptions[i] = spellPrefabList[i].GetComponent<Spell>().GetDescription();
        }

        InitializeSpellDictionary();
    }

    void InitializeSpellDictionary()
    {
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
    public Dictionary<string, string> GetSpellDictionary() => _spellDictionary;
    
}
