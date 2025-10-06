using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Start,     
    Playing,    
    GameOver,   
    Reset       
}

public class GameManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    public PlatformGenerator platformPrefab;
    public Transform playerBall;
    public Transform HelixContainer;

    [Header("UI")]
    public Button startGameButton;
    public Button newGameButton;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverPanel;

    [Header("Game Settings")]
    public float cylinderOffsetY = 22.5f;

    private List<GameObject> activeCylinders = new List<GameObject>();
    private int totalPlatformsCrossed = 0;
    private int currentScore = 0;
    private int highScore = 0;

    private GameState gameState;
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Rigidbody rb = playerBall.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Start()
    {
        SetGameState(GameState.Start);

        startGameButton.onClick.AddListener(StartGame);
        newGameButton.onClick.AddListener(ResetGame);

        LoadHighScore();
        UpdateScoreText();
    }

    void SetGameState(GameState state)
    {
        gameState = state;

        startGameButton.gameObject.SetActive(state == GameState.Start);
        newGameButton.gameObject.SetActive(state == GameState.Reset);
        gameOverPanel.SetActive(state == GameState.GameOver);
    }

    void StartGame()
    {
        SetGameState(GameState.Playing);
        SpawnInitialCylinder();

        Rigidbody rb = playerBall.GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void ResetGame()
    {
        foreach (var cyl in activeCylinders)
            Destroy(cyl);
        activeCylinders.Clear();

        totalPlatformsCrossed = 0;
        currentScore = 0;
        UpdateScoreText();

        Rigidbody rb = playerBall.GetComponent<Rigidbody>();
        rb.useGravity = false;

        playerBall.position = new Vector3(0f, 13.69f, -1.07f);
        playerBall.GetComponent<Rigidbody>().velocity = Vector3.zero;

        SetGameState(GameState.Start);
    }

    void SpawnInitialCylinder()
    {
        GameObject firstCylinder = Instantiate(platformPrefab.gameObject, HelixContainer);
        firstCylinder.transform.localPosition = Vector3.zero;
        firstCylinder.transform.localRotation = Quaternion.identity;
        firstCylinder.GetComponent<PlatformGenerator>().GeneratePlatforms();
        activeCylinders.Add(firstCylinder);
    }

    public void OnPlatformCrossed(GameObject crossedPlatform)
    {
        if (gameState != GameState.Playing) return;

        totalPlatformsCrossed++;
        currentScore++;
        UpdateScoreText();

        if (totalPlatformsCrossed % 10 == 6)
            SpawnNextCylinder();

        if (totalPlatformsCrossed % 10 == 3)
            DeletePreviousCylinder();
    }

    public void OnDeadlyHit()
    {
        if (gameState != GameState.Playing) return;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateScoreText();

        SetGameState(GameState.GameOver);
        gameOverPanel.SetActive(true);
        startGameButton.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(true);

    }

    private void SpawnNextCylinder()
    {
        GameObject lastCylinder = activeCylinders[activeCylinders.Count - 1];
        Vector3 newPos = lastCylinder.transform.localPosition - new Vector3(0, cylinderOffsetY, 0);
        GameObject newCylinder = Instantiate(platformPrefab.gameObject, HelixContainer.transform);
        newCylinder.transform.localPosition = newPos;
        newCylinder.transform.localRotation = Quaternion.identity;
        newCylinder.GetComponent<PlatformGenerator>().GeneratePlatforms();
        activeCylinders.Add(newCylinder);
    }

    private void DeletePreviousCylinder()
    {
        if (activeCylinders.Count > 1)
        {
            GameObject oldCylinder = activeCylinders[0];
            activeCylinders.RemoveAt(0);
            Destroy(oldCylinder);
        }
    }

    private void UpdateScoreText()
    {
        currentScoreText.text = "Score: " + currentScore;
        highScoreText.text = "High Score: " + highScore;
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
    }
}
