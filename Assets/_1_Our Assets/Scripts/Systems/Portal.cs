using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private int endScene = 3;
    [SerializeField] private Color fadeOutColor;
    [SerializeField] private float fadeOutDuration;
    private LocalSceneLoad _sceneLoader;

    private void Awake()
    {
        _sceneLoader = FindObjectOfType<LocalSceneLoad>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _sceneLoader.LoadScene(endScene, fadeOutColor, fadeOutDuration);
        }
    }
}
