using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI unitNumberText;
    [SerializeField] private int maximumUnitNumber = 5;
    public int currentUnitNumber;

    private GameManager gameManager;
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(ResumeGame);
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            maximumUnitNumber = gameManager.PartySize;
        }
    }

    private void Update()
    {
        if (gameManager != null)
        {
            currentUnitNumber = gameManager.UnitCount;
            if (unitNumberText != null)
            {
                unitNumberText.text = string.Format("Number of Unit: {0} / {1}", currentUnitNumber, maximumUnitNumber);
            }
        }
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming game");
        if (PauseManager.Instance != null)
            PauseManager.Instance.SetPaused(false);
        else
            Debug.LogWarning("PauseManager instance is missing");
    }

    public void PauseGame()
    {
        Debug.Log("Pausing game");
        if (PauseManager.Instance != null)
            PauseManager.Instance.SetPaused(true);
        else
            Debug.LogWarning("PauseManager instance is missing");
    }
}
