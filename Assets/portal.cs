using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    public void WarpGateActive()
    {
        SceneManager.LoadScene(3);
    }
    public void OnTriggerEnter(Collider other)
    {
        WarpGateActive();
    }
}
