using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar :MonoBehaviour {

    private Slider _slider;

    void Start() {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateHealthBar(float currentValue, float maxValue) {
        _slider.value = currentValue / maxValue;
    }
}
