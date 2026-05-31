using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [SerializeField] private Image[] hearts;

    [Header("UI Elements")]
    public TMP_Text feedbackText;
    public TMP_Text scoreText;
    public TMP_Text streakText;

    [Header("Educational Panel Settings")]
    public GameObject infoPanel;
    public TMP_Text infoHeaderText;
    public TMP_Text infoContentText;

    [Header("Room Cleaning")]
    [Tooltip("Starting score in Clean The Room mode.")]
    public int roomCleaningStartScore = 0;

    [Header("Game Settings")]

    public int maxConsecutiveErrors = 3;

    private int currentErrorStreak = 0;
    private int currentScore = 0;
    private bool isGameOver = false;

    public bool IsGameOver => isGameOver;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
        UpdateUI();
    }

    // --- ZDOBYWANIE PUNKTÓW  ---
    public void AddScore(int amount)
    {
        if (amount > 0 && GameData.CurrentGameMode != GameData.GameMode.MultiBin)
            currentErrorStreak = 0;

        currentScore += amount;

        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
        {
            UpdateUI();
            return;
        }

        if (currentScore < 0)
            currentScore = 0;

        UpdateUI();
    }

    // --- BŁĄD  ---
    public void AddMistake(TrashItem.TrashType problemType)
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
        {
            ShowRoomCleaningWrongBin(problemType);
            return;
        }

        currentErrorStreak++;
        UpdateUI();

        if (currentErrorStreak >= maxConsecutiveErrors)
        {
            ShowEducationalInfo(problemType);
        }
    }

    public void HandleRoomCleaningResult(bool correct, TrashItem.TrashType trashType)
    {
        if (correct)
        {
            ShowFeedbackMessage("Dobrze!", Color.green);
            AddScore(1);
            return;
        }

        ShowRoomCleaningWrongBin(trashType);
    }

    public void ConfigureForRoomCleaning()
    {
        currentErrorStreak = 0;
        currentScore = roomCleaningStartScore;
        isGameOver = false;

        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (streakText != null)
            streakText.gameObject.SetActive(false);

        foreach (Image heart in hearts)
        {
            if (heart != null)
                heart.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    void ShowRoomCleaningWrongBin(TrashItem.TrashType trashType)
    {
        ShowFeedbackMessage("Zły Kosz!", Color.red);
        AddScore(-1);

        if (currentScore < 0)
            ShowGameOver(trashType);
    }

    void ShowGameOver(TrashItem.TrashType trashType)
    {
        isGameOver = true;
        StopAllCoroutines();
        ShowEducationalInfo(trashType, pauseGame: true);
    }

    // --- WYŚWIETLANIE INFORMACJI ---
    void ShowEducationalInfo(TrashItem.TrashType type)
    {
        ShowEducationalInfo(type, pauseGame: true);
    }

    void ShowEducationalInfo(TrashItem.TrashType type, bool pauseGame)
    {
        StopAllCoroutines();

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        if (pauseGame)
            Time.timeScale = 0f;

        UpdateUI();

        string title = "";
        string content = "";

        switch (type)
        {
            case TrashItem.TrashType.Plastic:
                title = "Pojemnik: PLASTIK I METAL";
                content = "WRZUCAMY:\n- Zgniecione butelki PET\n- Opakowania po jogurtach\n- Puszki\n\nNIE WRZUCAMY:\n- Butelek z zawartością\n- Zabawek z bateriami";
                break;
            case TrashItem.TrashType.Paper:
                title = "Pojemnik: PAPIER";
                content = "WRZUCAMY:\n- Gazety, ulotki\n- Kartony\n\nNIE WRZUCAMY:\n- Paragonów\n- Tłustego papieru po pizzy";
                break;
            case TrashItem.TrashType.Glass:
                title = "Pojemnik: SZKŁO";
                content = "WRZUCAMY:\n- Szklane butelki\n- Słoiki\n\nNIE WRZUCAMY:\n- Ceramiki\n- Luster\n- Szyb";
                break;
            case TrashItem.TrashType.Organic:
                title = "Pojemnik: BIO";
                content = "WRZUCAMY:\n- Obierki z owoców\n- Fusy z kawy\n\nNIE WRZUCAMY:\n- Mięsa i kości\n- Odchodów zwierząt";
                break;
            default:
                title = "Ups!";
                content = "Uważaj, gdzie wyrzucasz śmieci!";
                break;
        }

        if (infoHeaderText != null) infoHeaderText.text = title;
        if (infoContentText != null) infoContentText.text = content;

        if (infoPanel != null) infoPanel.SetActive(true);
    }

    // --- PRZYCISKI ---
    public void ResumeGame()
    {
        if (isGameOver)
        {
            if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
                RestartRoomCleaning();
            return;
        }

        currentErrorStreak = 0;
        UpdateUI();

        if (infoPanel != null) infoPanel.SetActive(false);

        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SecondaryMenu");
    }

    public void RestartRoomCleaning()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CleanTheRoom");
    }

    // --- AKTUALIZACJA TEKSTÓW NA EKRANIE ---

    void UpdateHealthBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < (maxConsecutiveErrors - currentErrorStreak))
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void UpdateUI()
    {

        UpdateHealthBar();

        if (scoreText != null)
            scoreText.text = "Punkty: " + currentScore;

        if (streakText != null)
        {
            if (currentErrorStreak == 2) streakText.color = Color.red;
            else streakText.color = Color.white;

            streakText.text = $"Seria błędów: {currentErrorStreak} / {maxConsecutiveErrors}";
        }
    }

    public void ShowFeedbackMessage(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
            StopAllCoroutines();
            StartCoroutine(FadeText(color));
        }
    }

    private IEnumerator FadeText(Color startColor)
    {
        feedbackText.color = startColor;
        float timer = 0;
        float duration = 1.5f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / duration);
            feedbackText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        feedbackText.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }
}