using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] int fps = 60; 
   
    void Awake()
    {
        Application.targetFrameRate = fps;
    }

}
