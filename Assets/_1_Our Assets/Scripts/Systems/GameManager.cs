using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

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
    [SerializeField] private GameObject[] playerControllers;
    [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    private GameObject _player;
    private Camera _playerCamera;
    private string[] _spellNames;
    private string[] _spellDescriptions;
    private Dictionary<string, string> _spellDictionary = new();
    
    public FadeScreen fadeScreen;
    [SerializeField] private float rayEnableDelay = 0.5f;
    [SerializeField] private int dungeonSceneIndex = 2;
    [SerializeField] private int gameOverSceneIndex = 3;
    [SerializeField] private Color gameOverFadeColor;

    private void Awake()
    {
        _player = transform.root.GetComponentInChildren<Player>().gameObject;
        _playerCamera = _player.GetComponentInChildren<Camera>();
        InitializeSpellListsAndDictionary();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EndGame(bool defeat)
    {
        var localLoader = GameObject.FindGameObjectWithTag("LoadPosition").GetComponent<LocalSceneLoad>();
        
        if (defeat)
        {
            localLoader.LoadScene(gameOverSceneIndex, gameOverFadeColor, 2);
        }
        else
        {
            FindObjectOfType<Portal>(true).gameObject.SetActive(true);
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
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // Disables rays if dungeon scene
        foreach (var ray in playerRays)
        {
            if (scene.buildIndex == dungeonSceneIndex || scene.buildIndex == 0)
            {
                ray.SetActive(false);
            }
            else
            {
                StartCoroutine(EnableControllerAfterDelay(ray, true, true));
                _player.GetComponent<Player>().ResetDeath();
            }
        }
        foreach (var controller in playerControllers)
        {
            StartCoroutine(EnableControllerAfterDelay(controller, false, false));
        }
        
        var startTransform = GameObject.FindGameObjectWithTag("LoadPosition").transform;
        _player.transform.position = startTransform.position;
        _player.transform.rotation = startTransform.rotation;
        _playerCamera.transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

    private IEnumerator EnableControllerAfterDelay(GameObject ray, bool activate, bool isRay)
    {
        if (activate) ray.SetActive(false);
        yield return new WaitForSeconds(rayEnableDelay);
        if (activate) ray.SetActive(true);
        if (isRay)
        {
            ray.GetComponent<XRRayInteractor>().interactionManager = FindObjectOfType<XRInteractionManager>();
        }
        else
        {
            ray.GetComponent<XRDirectInteractor>().interactionManager = FindObjectOfType<XRInteractionManager>();
        }
    }
    
    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex, false, Color.black, 0));
    }

    public void GoToScene(int sceneIndex, Color fadeColor, float duration)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex, true, fadeColor, duration));
    }

    IEnumerator GoToSceneRoutine(int sceneIndex, bool hasColor, Color fadeColor, float duration)
    {
        if (hasColor) fadeScreen.FadeOut(fadeColor, duration);
        else fadeScreen.FadeOut();
        
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
    }
}
