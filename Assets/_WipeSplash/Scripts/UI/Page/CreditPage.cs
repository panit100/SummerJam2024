using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditPage : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject creditPage;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button backButton;

    void Start()
    {
        creditButton.onClick.AddListener(OpenCreditPage);
        backButton.onClick.AddListener(CloseCreditPage);
    }

    void OpenCreditPage()
    {
        creditPage.SetActive(true);
    }
    void CloseCreditPage()
    {
        creditPage.GetComponent<Animator>().Play("CreditExit");
        Invoke("WaitClose", 1f);
    }
    void WaitClose()
    {
        creditPage.SetActive(false);
    }

}