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


        if (_panelAttack != null) {
            _panelAttack.SetActive(true);
            _currentPanel = _panelAttack; // Lo asignamos como el panel actual
        }
    }
    public void toggleAttackPanel() {
        SwitchPanel(_panelAttack);
    }
    public void ToggleDefensePanel() {
        SwitchPanel(_panelDefense);
    }
    public void ToggleUtilityPanel() {
        SwitchPanel(_panelUtility);
    }
    private void SwitchPanel(GameObject newPanel) {
        // Si el panel que queremos mostrar ya es el actual, no hacemos nada
        if (_currentPanel == newPanel) {
            return;
        }

        // Si hay un panel activo, lo desactivamos
        if (_currentPanel != null) {
            _currentPanel.SetActive(false);
        }

        // Activamos el nuevo panel y actualizamos la referencia de _currentPanel
        if (newPanel != null) {
            newPanel.SetActive(true);
            _currentPanel = newPanel;
        }
    }
}