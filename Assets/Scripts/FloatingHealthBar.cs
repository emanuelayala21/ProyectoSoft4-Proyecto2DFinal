using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar :MonoBehaviour {

    public Slider slider;

    public void UpdateHealthBar(float currentValue, float maxValue) {
        slider.value = currentValue / maxValue;

        if(currentValue <= 0) {
            slider.gameObject.SetActive(false);
        }
    }
}
