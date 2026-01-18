using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    public TMP_Text feedbackText;     
    public TMP_Text scoreText;        
    public TMP_Text streakText;       

    [Header("Educational Panel Settings")]
    public GameObject infoPanel;     
    public TMP_Text infoHeaderText;   
    public TMP_Text infoContentText;  

    [Header("Game Settings")]
    public int maxConsecutiveErrors = 3; 
    private int currentStreak = 0;      
    private int currentScore = 0;

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

        if (amount > 0)
        {
            currentStreak = 0;
        }

        currentScore += amount;
        if (currentScore < 0) currentScore = 0;

        UpdateUI();
    }

    // --- BŁĄD  ---
    public void AddMistake(TrashItem.TrashType problemType)
    {
        currentStreak++; 
        UpdateUI();

 
        if (currentStreak >= maxConsecutiveErrors)
        {
            ShowEducationalInfo(problemType);
        }
    }

    // --- WYŚWIETLANIE INFORMACJI ---
    void ShowEducationalInfo(TrashItem.TrashType type)
    {
        StopAllCoroutines();

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

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
        currentStreak = 0; 
        UpdateUI();

        if (infoPanel != null) infoPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SecondaryMenu");
    }

    // --- AKTUALIZACJA TEKSTÓW NA EKRANIE ---
    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Punkty: " + currentScore;

        if (streakText != null)
        {
            if (currentStreak == 2) streakText.color = Color.red;
            else streakText.color = Color.white;

            streakText.text = $"Seria błędów: {currentStreak} / {maxConsecutiveErrors}";
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