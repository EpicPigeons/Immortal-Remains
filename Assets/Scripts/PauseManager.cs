using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; set; }
    public bool IsPaused { get; set; } = true;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    public void SetPaused(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0f;
            IsPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            IsPaused = false;
        }
        Debug.Log("Paused: " + IsPaused);
    }

    public void TogglePause()
    {
        SetPaused(!IsPaused);
    }
}
