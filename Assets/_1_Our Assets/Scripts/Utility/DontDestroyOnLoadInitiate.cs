using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadInitiate : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
