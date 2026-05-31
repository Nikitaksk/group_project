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

    [Header("Educational Panel")]
    public GameObject infoPanel;
    public TMP_Text infoHeaderText;
    public TMP_Text infoContentText;

    // --- NOWA SEKCJA: EKRAN WYGRANEJ ---
    [Header("Win Panel Settings")]
    public GameObject winPanel;
    public TMP_Text finalScoreText;
    // -----------------------------------

    [Header("Room Cleaning")]
    [Tooltip("Starting score in Clean The Room mode.")]
    public int roomCleaningStartScore = 0;

    [Header("Game Settings")]
    public int maxConsecutiveErrors = 3;

    private int currentErrorStreak;
    private int currentScore;
    private bool isGameOver;

    public bool IsGameOver => isGameOver;

    // --- NOWA ZMIENNA: Ilość śmieci w pokoju ---
    private int trashRemainingInRoom;
    // ------------------------------------------

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);

        // --- Ukrywamy ekran wygranej na starcie ---
        if (winPanel != null)
            winPanel.SetActive(false);
        // ------------------------------------------

        UpdateUI();
    }

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

    public void AddMistake(TrashItem.TrashType problemType)
    {
        currentErrorStreak++;
        UpdateUI();

        if (currentErrorStreak >= maxConsecutiveErrors)
            ShowEducationalInfo(problemType);
    }

    public void HandleRoomCleaningResult(bool correct, TrashItem.TrashType trashType)
    {
        if (correct)
        {
            ShowFeedbackMessage("Dobrze!", Color.green);
            AddScore(1);

            // --- AKTUALIZACJA POSTĘPU W SPRZĄTANIU ---
            trashRemainingInRoom--;
            if (trashRemainingInRoom <= 0)
            {
                ShowWinScreen();
            }
            // -----------------------------------------
            return;
        }

        ShowFeedbackMessage("Zły Kosz!", Color.red);
        AddScore(-1);

        if (currentScore < 0)
            EndRoomCleaningGame(trashType);
    }

    public void ConfigureForRoomCleaning()
    {
        currentErrorStreak = 0;
        currentScore = roomCleaningStartScore;
        isGameOver = false;

        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (streakText != null)
            streakText.gameObject.SetActive(false);

        foreach (Image heart in hearts)
        {
            if (heart != null)
                heart.gameObject.SetActive(false);
        }

        // --- POLICZ ŚMIECI NA STARCIE POKOJU ---
        TrashItem[] allTrash = FindObjectsOfType<TrashItem>();
        trashRemainingInRoom = allTrash.Length;
        // ---------------------------------------

        UpdateUI();
    }

    void EndRoomCleaningGame(TrashItem.TrashType trashType)
    {
        isGameOver = true;
        StopAllCoroutines();
        ShowEducationalInfo(trashType);
    }

    // --- NOWA FUNKCJA: POKAŻ EKRAN WYGRANEJ ---
    private void ShowWinScreen()
    {
        isGameOver = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Zdobyte punkty: " + currentScore;

        // Możesz tu zatrzymać czas, jeśli chcesz
        // Time.timeScale = 0f; 
    }
    // ------------------------------------------

    void ShowEducationalInfo(TrashItem.TrashType type)
    {
        StopAllCoroutines();

        if (feedbackText != null)
            feedbackText.text = "";

        Time.timeScale = 0f;
        UpdateUI();

        GetEducationalContent(type, out string title, out string content);

        if (infoHeaderText != null) infoHeaderText.text = title;
        if (infoContentText != null) infoContentText.text = content;
        if (infoPanel != null) infoPanel.SetActive(true);
    }

    static void GetEducationalContent(TrashItem.TrashType type, out string title, out string content)
    {
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
    }

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

        if (infoPanel != null)
            infoPanel.SetActive(false);

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

    void UpdateUI()
    {
        if (GameData.CurrentGameMode != GameData.GameMode.MultiBin)
            UpdateHealthBar();

        if (scoreText != null)
            scoreText.text = "Punkty: " + currentScore;

        if (streakText != null && streakText.gameObject.activeSelf)
        {
            streakText.color = currentErrorStreak == 2 ? Color.red : Color.white;
            streakText.text = $"Seria błędów: {currentErrorStreak} / {maxConsecutiveErrors}";
        }
    }

    void UpdateHealthBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].enabled = i < maxConsecutiveErrors - currentErrorStreak;
        }
    }

    public void ShowFeedbackMessage(string message, Color color)
    {
        if (feedbackText == null)
            return;

        feedbackText.text = message;
        feedbackText.color = color;
        StopAllCoroutines();
        StartCoroutine(FadeText(color));
    }

    IEnumerator FadeText(Color startColor)
    {
        feedbackText.color = startColor;
        float timer = 0f;
        const float duration = 1.5f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            feedbackText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        feedbackText.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}