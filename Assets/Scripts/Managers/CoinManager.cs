using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coins = 0;
    public int coinsToWin = 10;

    public TextMeshProUGUI coinText;
    public GameObject winText;

    private void Awake()
    {
        instance = this;
    }

    public void AddCoin()
    {
        coins++;
        coinText.text = "Coins: " + coins;

        if (coins >= coinsToWin)
        {
            winText.SetActive(true);
            Time.timeScale = 0f;
            StartCoroutine(WaitForAnyKeyWin());
        }
    }

    IEnumerator WaitForAnyKeyWin()
    {
        while (!Keyboard.current.anyKey.wasPressedThisFrame)
        {
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
