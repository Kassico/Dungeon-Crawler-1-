using UnityEngine;

public class StatsPanelManeger : MonoBehaviour
{
    private GameObject statsPanel;

    void Start()
        {
            statsPanel = GameObject.Find("PlayerStatsPanel");
    }

    public void ShowPanel() // när kallad på aktiverar statspanel
    {         
        statsPanel.SetActive(true);

    }

    public void HidePanel() // när kallad på inaktiverar statspanel
    {
        statsPanel.SetActive(false);
    }


}
