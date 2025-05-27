using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocalSceneLoad : MonoBehaviour
{
    [SerializeField] private bool loadOnAwake;
    [SerializeField] private int loadOnAwakeIndex;
    [SerializeField] private TextMeshProUGUI contSpeedDisplayText;
    private GameManager _gameManager;
    private Player _player;
    private ActionBasedContinuousTurnProvider _contProvider;
    
    private void Start()
    {
        if (loadOnAwake)
        {
            LoadScene(loadOnAwakeIndex);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        _gameManager ??= FindObjectOfType<GameManager>();
        _gameManager.GoToScene(sceneIndex);
    }
    public void LoadScene(int sceneIndex, Color fadeColor, float duration)
    {
        _gameManager ??= FindObjectOfType<GameManager>();
        _gameManager.GoToScene(sceneIndex, fadeColor, duration);
    }

    public void SetSnapTurn(bool enable)
    {
        _player ??= FindObjectOfType<Player>();
        _player.GetComponent<ActionBasedSnapTurnProvider>().enabled = enable;
    }

    public void SetContinuousTurn(bool enable)
    {
        _player ??= FindObjectOfType<Player>();
        _player.GetComponent<ActionBasedContinuousTurnProvider>().enabled = enable;
    }

    public void SetContinuousTurnSpeed(float speed)
    {
        _player ??= FindObjectOfType<Player>();
        _contProvider ??= _player.GetComponent<ActionBasedContinuousTurnProvider>();
        _contProvider.turnSpeed = speed;
        contSpeedDisplayText.text = speed + "";
    }

    public void SetLeftHanded(bool enable)
    {
        _gameManager ??= FindObjectOfType<GameManager>();
        if (enable)
        {
            _gameManager.SetDominantHand(DominantHand.LeftHanded);
        }
    }

    public void SetRightHanded(bool enable)
    {
        _gameManager ??= FindObjectOfType<GameManager>();
        if (enable)
        {
            _gameManager.SetDominantHand(DominantHand.RightHanded);
        }
    }
}
