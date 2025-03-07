using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager :MonoBehaviour {

    public TextMeshProUGUI textAmountCoins;
    public TextMeshProUGUI textPrice;
    public TextMeshProUGUI textActualStat;
    public TextMeshProUGUI textUpgradeStat;

    public TextMeshProUGUI textNoFunds;
    public TextMeshProUGUI textBuyConfirmation;

    public GameObject _panelGameOver;
    public GameObject _panelNewLevel;

    void Start() {

        // Initialize the texts with a placeholder
        textPrice.text = "?";
        textActualStat.text = "?";
        textUpgradeStat.text = "+?";
    }

    public void ShowCoinsUI(int amount) {
        textAmountCoins.text = "Monedas: " + amount; // Display the current coin amount in the UI
    }
    public void ShowPriceBoost(int price, float statAmount, float currentAmount) {
        float upgradeAmount = statAmount - currentAmount; // Calculate the upgrade amount
        textPrice.text = price.ToString(); // Display the price of the upgrade
        textActualStat.text = currentAmount.ToString(); // Display the current stat value

        if(upgradeAmount > 0) { // If the upgrade amount is positive
            textUpgradeStat.text = "+ " + upgradeAmount.ToString("F1"); // Show the upgrade amount with 1 decimal place
        } else { // If the upgrade amount is negative
            upgradeAmount *= -1; // Make the upgrade amount positive
            textUpgradeStat.text = "- " + upgradeAmount.ToString("F2"); // Show the negative upgrade amount with 2 decimal places
        }
    }
    public void ShowNoFundsMsg() {
        textNoFunds.gameObject.SetActive(true); // Display a "no funds" message
        StartCoroutine(DisableNoFundsMsg()); // Start a coroutine to disable the message after a short delay
    }
    private IEnumerator DisableNoFundsMsg() {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before hiding the "no funds" message
        textNoFunds.gameObject.SetActive(false); // Hide the "no funds" message
    }
    public void ShowBuyConfMsg() {
        textBuyConfirmation.gameObject.SetActive(true); // Display a "no funds" message
        StartCoroutine(DisableBuyConfMsg()); // Start a coroutine to disable the message after a short delay
    }
    private IEnumerator DisableBuyConfMsg() {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before hiding the "no funds" message
        textBuyConfirmation.gameObject.SetActive(false); // Hide the "no funds" message
    }
    public void GameOver() {
        _panelGameOver.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void LevelPassed() {
        _panelNewLevel.gameObject.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(DisableLevelPassed());
    }
    private IEnumerator DisableLevelPassed() {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before hiding the "no funds" message
        _panelNewLevel.gameObject.SetActive(false);
        Time.timeScale = 1f;

    }

    public void RestartGame() {
        _panelGameOver.gameObject.SetActive(false);
        GameManager.Instance.ResetGame();

    }
    public void StartGame() {
        SceneManager.LoadScene("MainGameplay");
        GameManager.Instance.InitializeGame();
    }
     public void GoMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame() {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}