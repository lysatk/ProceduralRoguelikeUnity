using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainView;
    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {

        settingsPanel.SetActive(true);
        mainView.SetActive(false );



    }

    public void HideSettings()
    {

        settingsPanel.SetActive(false);
        mainView?.SetActive(true);


    }
}
