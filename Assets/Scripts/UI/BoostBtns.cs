using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBtns : MonoBehaviour {

    private MainHouse player;
    private int[] _boostCost = new int[] { 1, 2, 4, 6, 9, 12, 16, 20, 25, 32 };
    void Start() {
        player = FindObjectOfType<MainHouse>();
    }

    public void test(int costo) {

    }
    
}
