using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] public List<GameObject> uiPanels;

    public void ChangeUiPanel(int id)
    {
        HideAllUiPanes();
        uiPanels[id].SetActive(true);
    }

    public void HideAllUiPanes()
    {
        foreach (GameObject uiPanel in uiPanels)
        {
            uiPanel.SetActive(false);
        }
    }
}

