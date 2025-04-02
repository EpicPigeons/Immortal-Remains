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

    [SerializeField] private TextMeshProUGUI unitHealthText;

    private int currentUnitNumber = 0;
    private int maximumUnitNumber = 0;

    private GameManager gameManager;

    public static UIManager Instance;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Button pbtn = pauseButton.GetComponent<Button>();
        pbtn.onClick.AddListener(pauseButtonClicked);

        Button plbtn = playButton.GetComponent<Button>();
        plbtn.onClick.AddListener(playButtonClicked);

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            maximumUnitNumber = gameManager.PartySize;
        }
    }

    void Update()
    {
        unitNumberText.text = string.Format("Number of Unit: {0} / {1}", currentUnitNumber, maximumUnitNumber);
    }

    public void IncreaseUnitCount()
    {
        if (currentUnitNumber < maximumUnitNumber)
        {
            currentUnitNumber++;
        }
        else
        {
            Debug.Log("Unit limit reached!");
        }
    }

    public void ResetUnitCount()
    {
        currentUnitNumber = 0;
    }

    public void UpdateUnitHealthUI(float currentHealth, float maxHealth)
    {
        if (unitHealthText != null)
        {
            unitHealthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    public void playButtonClicked()
    {
        Debug.Log("play Pressed");
        SceneManager.LoadScene(level);
    }

    public void pauseButtonClicked()
    {
        Debug.Log("pause Pressed");
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            Debug.Log("Game is paused.");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Game is resumed.");
        }
    }
}
