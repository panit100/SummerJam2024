using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button creditButton;
    [SerializeField] Canvas subcanvas;
    [SerializeField] LoadingManager loadingMAG;
    [SerializeField] GraphicRaycaster subcanvasRaycaster;
    GameObject subcanvasOBJ;
    const string BGMmainmenu = "MainMenu_RuDooRon";
    void Start()
    {
        SoundManager.Instance.ChangeBGM(BGMmainmenu);
        subcanvasOBJ = subcanvas.gameObject;
        startButton.onClick.AddListener(ClickToGame);
        creditButton.onClick.AddListener(delegate{});
    }

    void ClickToGame()
    {
        loadingMAG.OnLoadingComplete += () => ToggleMainMenu(false);
        subcanvasRaycaster.enabled = false;
        GameManager.Instance.OnChangeState(GAMESTATE.INVENTORY);
        loadingMAG.DoLoading();
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
