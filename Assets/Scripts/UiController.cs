using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] public List<GameObject> uiPanels;

    // Switch panels
    public void ChangeUiPanel(int id)
    {
        HideAllUiPanes();
        uiPanels[id].SetActive(true);
    }

    // Hide all panels
    public void HideAllUiPanes()
    {
        foreach (GameObject uiPanel in uiPanels)
        {
            uiPanel.SetActive(false);
        }
    }
}

