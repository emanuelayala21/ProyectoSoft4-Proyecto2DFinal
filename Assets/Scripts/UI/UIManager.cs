using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI textAmountCoins;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void IncreaseCoinUI(int amount) {
        textAmountCoins.text = "Monedas: " + amount;
    }
}
