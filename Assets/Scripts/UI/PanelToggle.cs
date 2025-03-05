using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggle : MonoBehaviour {

    private GameObject _panelAttack;
    private GameObject _panelDefense;
    private GameObject _panelUtility;

    private GameObject _currentPanel;
    void Start() {
        _panelAttack = GameObject.Find("Panel-AttackButtons");
        _panelDefense = GameObject.Find("Panel-DefenseButtons");
        _panelUtility = GameObject.Find("Panel-UtilitiesButtons");

        if (_panelAttack != null) _panelAttack.SetActive(false);
        if (_panelDefense != null) _panelDefense.SetActive(false);
        if (_panelUtility != null) _panelUtility.SetActive(false);


        if(_panelAttack != null) { // If attack panel is found
            _panelAttack.SetActive(true); // Activate attack panel
            _currentPanel = _panelAttack; // Set attack panel as the current active panel
        }
    }
    public void toggleAttackPanel() {
        SwitchPanel(_panelAttack); // Switch to the attack panel
    }
    public void ToggleDefensePanel() {
        SwitchPanel(_panelDefense); // Switch to the defense panel
    }
    public void ToggleUtilityPanel() {
        SwitchPanel(_panelUtility); // Switch to the utility panel
    }
    private void SwitchPanel(GameObject newPanel) {
        if(_currentPanel == newPanel) {  // If the new panel is already the current one, do nothing
            return;
        }
        if(_currentPanel != null) {  // If there is an active panel, deactivate it
            _currentPanel.SetActive(false);
        }
        if(newPanel != null) {  // Activate the new panel and set it as the current panel
            newPanel.SetActive(true);
            _currentPanel = newPanel;
        }
    }
}