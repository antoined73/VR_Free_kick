using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public CanvasGroup menuUI;

    public CanvasGroup defenderWallUI;

    private GameObject gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        TurnOffCanvas(menuUI);
    }

    public void ClickOnMenu(string role)
    {
        TurnOffCanvas(menuUI);
    }

    private void TurnOnCanvas(CanvasGroup canvas)
    {
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }

    private void TurnOffCanvas(CanvasGroup canvas)
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
    }
}
