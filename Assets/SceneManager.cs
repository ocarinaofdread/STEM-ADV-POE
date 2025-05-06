using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public void GoToScene(int scemeIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));   
    }
   
    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        fadeScreen.Fadeout();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
    https://www.youtube.com/watch?v=JCyJ26cIM0Y
    }
}
