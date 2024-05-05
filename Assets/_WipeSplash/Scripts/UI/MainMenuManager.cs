using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button creditButton;
    [SerializeField] Button backcreditButton;
    [SerializeField] Canvas subcanvas;
    [SerializeField] LoadingManager loadingMAG;
    [SerializeField] GraphicRaycaster subcanvasRaycaster;

    [SerializeField] CanvasGroup credit;
    GameObject subcanvasOBJ;
    const string BGMmainmenu = "MainMenu_RuDooRon";
    void Start()
    {
        SoundManager.Instance.ChangeBGM(BGMmainmenu);
        subcanvasOBJ = subcanvas.gameObject;
        startButton.onClick.AddListener(ClickToGame);
        creditButton.onClick.AddListener(clickTocredit);
        if (backcreditButton != null)
            backcreditButton.onClick.AddListener(creditToMenu);
        credit.alpha = 0;
        credit.blocksRaycasts = false;
    }

    void ClickToGame()
    {
        loadingMAG.OnLoadingComplete += () => ToggleMainMenu(false);
        loadingMAG.OnLoadingComplete += () => GameManager.Instance.OnChangeState(GAMESTATE.DIALOG);
        subcanvasRaycaster.enabled = false;
        loadingMAG.DoLoading();
    }

    void clickTocredit()
    {
        credit.alpha = 1;
        credit.blocksRaycasts = true;
    }
    void creditToMenu()
    {
        credit.alpha = 0;
        credit.blocksRaycasts = false;
    }

    void ToggleMainMenu(bool isToggle)
    {
        if (isToggle)
        {
            subcanvas.enabled = true;
            subcanvasOBJ.SetActive(true);
            subcanvasRaycaster.enabled = true;
        }
        else
        {
            subcanvas.enabled = false;
            subcanvasOBJ.SetActive(false);
            subcanvasRaycaster.enabled = false;
        }
    }
}
