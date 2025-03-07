using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBtns :MonoBehaviour {

    private MainHouse player;
    public UIManager uiManager;

    private int _selectedBoost = 1;

    private int[] _boostCost = new int[] { 1, 2, 4, 6, 9, 12, 16, 20, 25, 32 };

    ///Attack Boost
    private float[] _damageBoost = new float[] { 3, 4, 6, 8, 11, 15, 20, 26, 33, 41 };
    private float[] _fireRateBoost = new float[] { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.42f, 0.35f, 0.3f, 0.25f };
    private float[] _critChanceBoost = new float[] { 1f, 2.5f, 4f, 5f, 7f, 8f, 10f, 11f, 15f, 18.5f };
    private float[] _rangeBoost = new float[] { 4.7f, 5.0f, 5.5f, 6.2f, 7.0f, 8.0f, 9.2f, 10.5f, 12.0f, 14.0f };

    ///Defense Boost
    private float[] _liveMaxBoost = new float[] { 50, 60, 72, 86, 102, 120, 140, 162, 186, 200 };
    private float[] _regenBoost = new float[] { 0.5f, 1.0f, 1.5f, 2.2f, 3.0f, 3.8f, 4.7f, 5.5f, 6.5f, 7.5f };
    private float[] _knockbackBoost = new float[] { 0.0f, 0.01f, 0.03f, 0.05f, 0.07f, 0.09f, 1.2f, 1.5f, 2f, 2.4f };

    ///Utility Boost 

    void Start() {
        player = FindObjectOfType<MainHouse>();
    }
    public void TypeBoostBtn(int upgradeType) {
        _selectedBoost = upgradeType; // Store the selected upgrade type
        (int price, float nextValue, float currentBoost) = GetNextBoostValue(upgradeType); // Get the price, next boost value, and current boost value for the selected upgrade
        uiManager.ShowPriceBoost(price, nextValue, currentBoost); // Display the price and boost values in the UI
    }
    public void ApplyBoost() {
        (int price, float nextValue, float currentBoost) = GetNextBoostValue(_selectedBoost); // Get the price, next value, and current boost for the selected upgrade

        float upgradeAmount = nextValue - currentBoost; // Calculate the upgrade amount

        if(player.BuyUpgrades(price, upgradeAmount, _selectedBoost)) { // If the player can afford the upgrade
            TypeBoostBtn(_selectedBoost); // Refresh the UI with the new boost information
            uiManager.ShowBuyConfMsg();
        } else {
            uiManager.ShowNoFundsMsg(); // Show a message if the player doesn't have enough funds
        }
    }
    private (int, float, float) GetNextBoostValue(int boostType) {
        float[] boostArray = GetBoostArray(boostType); // Get the corresponding boost array based on the upgrade type
        if(boostArray == null) return (0, 0, 0); // Return default values if the boost array is null

        float currentBoost = GetCurrentBoostValue(boostType); // Get the current value of the selected boost
        int index = System.Array.IndexOf(boostArray, currentBoost); // Find the index of the current boost in the array

        if(index == -1 || index + 1 >= _boostCost.Length) return (0, currentBoost, 0); // Return default values if no valid next boost is found

        int price = _boostCost[index + 1]; // Get the price for the next boost
        float nextBoostValue = boostArray[index + 1]; // Get the next boost value

        return (price, nextBoostValue, currentBoost); // Return the price, next value, and current boost
    }
    private float GetCurrentBoostValue(int boostType) {
        switch(boostType) {
            case 0: return player.damage;         // Current damage value
            case 1: return player.fireRate;       // Current fire rate value
            case 2: return player.criticChance;   // Current critical chance value
            case 3: return player.fireRange;      // Current fire range value
            case 4: return player.healthMax;      // Current max health value
            case 5: return player.healthRegen; // Current life regeneration value
            case 6: return player.knockback;      // Current knockback value
            default: return 0; // Return 0 if no valid boost type is found
        }
    }
    private float[] GetBoostArray(int boostType) {
        switch(boostType) {
            case 0: return _damageBoost;       // Damage boost array
            case 1: return _fireRateBoost;     // Fire rate boost array
            case 2: return _critChanceBoost;   // Critical chance boost array
            case 3: return _rangeBoost;        // Fire range boost array
            case 4: return _liveMaxBoost;      // Max health boost array
            case 5: return _regenBoost;        // Life regeneration boost array
            case 6: return _knockbackBoost;    // Knockback boost array
            default: return null; // Return null if no valid boost type is found
        }
    }
}