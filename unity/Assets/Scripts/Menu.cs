using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public CanvasGroup menuUI;

    public CanvasGroup defenderWallUI;

    private GameObject gameController;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnMenu(string role)
    {
        TurnOffCanvas(menuUI);
        switch (role)
        {
            case "buteur":
                break;
            case "mur":
                TurnOnCanvas(defenderWallUI);
                break;
            case "gardien":
                break;
        }
        gameController.GetComponent<GameManager>().choiceRole(role);
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
