using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject leaderboardPanel;
    public GameObject shopPanel;
    public GameObject creditPanel;

    void Start()
    {
        ShowMain();
    }

    void HideAll()
    {
        mainPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        shopPanel.SetActive(false);
        creditPanel.SetActive(false);
    }

    void ShowMain()
    {
        HideAll();
        mainPanel.SetActive(true);
    }

    public void OpenLeaderboard()
    {
        HideAll();
        leaderboardPanel.SetActive(true);

        AnalyticManager.Instance.OnOpenLeaderboard();
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        mainPanel.SetActive(true);

        AnalyticManager.Instance.OnCloseLeaderboard();
    }

    public void OpenShop()
    {
        HideAll();
        shopPanel.SetActive(true);

        AnalyticManager.Instance.OnShopPressed();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OpenCredit()
    {
        HideAll();
        creditPanel.SetActive(true);

        AnalyticManager.Instance.OnCreditOpened();
    }

    public void CloseCredit()
    {
        creditPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}