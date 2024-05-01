using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndGamePage : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private Button endGameButton;
    [SerializeField] private Transform endGameHolder;
    [SerializeField] private Image transitionImage;
    [SerializeField] private GameObject transitionEndGame;

    private Sequence startSQ;

    void Start()
    {
        SetupUI();
    }
    void SetupUI()
    {
        endGameButton.onClick.AddListener(ReturnToMainMenu);
        endGameHolder.localScale = new Vector3();
        endGameButton.gameObject.SetActive(false);
        endGameCanvas.SetActive(false);

        transitionImage.color = new Color(0, 0, 0, 1);
        transitionImage.gameObject.SetActive(false);

        transitionEndGame.SetActive(false);
    }

    public void ActiveEndGame()
    {
        endGameCanvas.SetActive(true);
        transitionImage.gameObject.SetActive(true);

        startSQ = DOTween.Sequence();

        startSQ.Append(transitionImage.DOFade(0, 5f));
        startSQ.AppendCallback(() => transitionImage.gameObject.SetActive(false));
        startSQ.AppendInterval(1.5f);
        startSQ.AppendCallback(() => endGameButton.gameObject.SetActive(true));
        startSQ.Append(endGameHolder.DOScale(new Vector3(1, 1, 1) , 2.5f).SetEase(Ease.OutBounce));

        startSQ.Play();
    }

    void ReturnToMainMenu()
    {
        transitionEndGame.SetActive(true);
        Invoke("LoadMainMenuScene", 5f);
    }
    void LoadMainMenuScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}