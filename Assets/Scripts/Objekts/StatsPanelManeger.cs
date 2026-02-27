using UnityEngine;

public class StatsPanelManeger : MonoBehaviour
{
    private GameObject statsPanel;

    void Start()
        {
            statsPanel = GameObject.Find("PlayerStatsPanel");
    }

    public void ShowPanel()
    {         
        statsPanel.SetActive(true);

    }

    public void HidePanel()
    {
        statsPanel.SetActive(false);
    }

    //public void TogglePanel()
    //{
    //    statsPanel.SetActive(!statsPanel.activeSelf);
    //}

}
