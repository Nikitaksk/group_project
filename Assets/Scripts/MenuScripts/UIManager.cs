using System.Collections;
using UnityEngine;
using TMPro; // Make sure to include this for TextMeshPro

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton instance

    public TMP_Text feedbackText;
    public TMP_Text scoreText;
    public float fadeDuration = 1.5f;
    private int currentScore = 0;

    void Awake()
    {
        // Singleton pattern setup
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (currentScore < 0)
        {
            currentScore = 0;
        }

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Punkty: " + currentScore;
        }
    }

    // Call this method from other scripts to show a message
    public void ShowFeedbackMessage(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            StopAllCoroutines();
            // Start the fade-in/fade-out coroutine
            StartCoroutine(FadeText(color));
        }
    }

    private IEnumerator FadeText(Color startColor)
    {
        // Set start color and make it visible
        feedbackText.color = startColor;
        
        // Fade out
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            feedbackText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent at the end
        feedbackText.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }
}
