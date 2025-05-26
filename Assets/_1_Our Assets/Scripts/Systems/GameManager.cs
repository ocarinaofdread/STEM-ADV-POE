using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject[] playerRays;
    
    private string[] _spellNames;
    private string[] _spellDescriptions;
    private Dictionary<string, string> _spellDictionary = new();
    
    public FadeScreen fadeScreen;

    private void Awake()
    {
        InitializeSpellListsAndDictionary();
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // Disables rays if dungeon scene
        if (scene.buildIndex == 1)
        {
            foreach (var ray in playerRays)
            {
                ray.SetActive(false);
            }
        }
        else
        {
            foreach (var ray in playerRays)
            {
                ray.SetActive(true);
            }
        }
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
    
    // Scene Management
    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine( sceneIndex));
    }

    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        SceneManager.LoadScene(sceneIndex);
    }
}
