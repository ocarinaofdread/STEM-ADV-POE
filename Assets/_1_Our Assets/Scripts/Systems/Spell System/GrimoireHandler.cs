using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GrimoireHandler : MonoBehaviour
{
    // Dominant Hand: Controls
    // Non-Dominant Hand: Spells
    [SerializeField] private TextMeshProUGUI leftPageTMPro;
    [SerializeField] private TextMeshProUGUI rightPageTMPro;
    
    [TextArea(1, 3)] [SerializeField] private string controlsText;
    [SerializeField] private AudioClip leftFlipAudioClip;
    [SerializeField] private AudioClip rightFlipAudioClip;

    // <Name, Description>
    private Dictionary<string, string> _spellDictionary;
    private GameManager _gameManager;
    private DominantHand _currentDominantHand;
    private AudioSource _audioSource;
    
    private TextMeshProUGUI _currentSpellPage;
    private TextMeshProUGUI _currentControlsPage;

    private XRGrabInteractable _grabInteractable;
    private Animator _animator;
    private AnimatorUnityEventHandler _animatorUnityEventHandler;
    private readonly int _openHash = Animator.StringToHash("Open");
    private readonly int _closeHash = Animator.StringToHash("Close");
    
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _spellDictionary = _gameManager.GetSpellDictionary();
        _audioSource = GetComponentInChildren<AudioSource>();
        
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
            _gameManager ??= FindObjectOfType<GameManager>();
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
    
    // Socket Interaction Prevention
    
    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _animator = GetComponent<Animator>();
        _animatorUnityEventHandler = GetComponent<AnimatorUnityEventHandler>();
        
        _grabInteractable.selectEntered.AddListener(GrabInteractable_SelectEntered);
        _grabInteractable.selectExited.AddListener(GrabInteractable_SelectExited);
    }
    
    private void OnDestroy()
    {
        _grabInteractable.selectEntered.RemoveListener(GrabInteractable_SelectEntered);
        _grabInteractable.selectExited.RemoveListener(GrabInteractable_SelectExited);
    }
    
    private void GrabInteractable_SelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is not XRDirectInteractor) return;
        
        _animator.SetTrigger(_openHash);
        _animatorUnityEventHandler.SetBool(true);
    }
    private void GrabInteractable_SelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject is not XRDirectInteractor) return;
        
        _animator.SetTrigger(_closeHash);
         _animatorUnityEventHandler.SetBool(false);
    }
    
    public void PlayLeftFlipClip() { _audioSource.PlayOneShot(leftFlipAudioClip); }
    public void PlayRightFlipClip() { _audioSource.PlayOneShot(rightFlipAudioClip); }
}
