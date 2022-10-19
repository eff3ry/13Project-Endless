using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] public List<GameObject> uiPanels;

    //switch panels
    public void ChangeUiPanel(int id)
    {
        HideAllUiPanes();
        uiPanels[id].SetActive(true);
    }

    //hide all panels
    public void HideAllUiPanes()
    {
        foreach (GameObject uiPanel in uiPanels)
        {
            uiPanel.SetActive(false);
        }
    }
}

