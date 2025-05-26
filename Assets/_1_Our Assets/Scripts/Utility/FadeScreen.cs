using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public float fadeDuration = 2f;
    
    [SerializeField] private bool fadeOnStart = true;
    [SerializeField] private Color fadeColor;

    private Renderer _renderer;
    
    public void FadeIn()
    {
        Fade(1, 0);
    }
    public void FadeOut()
    {
        Fade(0, 1);
    }
    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        _renderer.enabled = true;
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor= fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration);
            _renderer.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null;
        }
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        _renderer.material.SetColor("_Color", newColor2);
        if (alphaOut == 0)
        {
            _renderer.enabled = false;
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _renderer ??= GetComponent<Renderer>();
        if(fadeOnStart)
        {
            Debug.Log("ONSceneLoad: fadeOnStart commenced");
            FadeIn();
        }
    }
}
