using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private GameObject gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    private void Start()
    {
        ShowMenu(true);
    }

    public void ChoiceRole(string roleString)
    {
        Role role;

        if (System.Enum.TryParse<Role>(roleString, out role))
        {
            ShowMenu(false);
            gameController.GetComponent<GameManager>().choiceRole(role);
        }
    }

    private void ShowMenu(bool show)
    {
        canvasGroup.alpha = (show? 1 : 0);
        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
    }
}
