using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public TextMeshProUGUI livesText;
    public GameObject gameOverText;

    void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
        gameOverText.SetActive(false);
    }

    public void TakeDamage()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    void UpdateLivesUI()
    {
        livesText.text = "Vidas: " + currentLives;
    }

    void GameOver()
    {
        gameOverText.SetActive(true);
        Time.timeScale = 0f;

        StartCoroutine(WaitForAnyKey());
    }

    IEnumerator WaitForAnyKey()
    {
        // Espera a que se pulse cualquier tecla del nuevo Input System
        while (!Keyboard.current.anyKey.wasPressedThisFrame)
        {
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
