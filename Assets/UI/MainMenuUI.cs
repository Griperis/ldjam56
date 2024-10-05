using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private GameManager manager;

    private Button startButton;
    private Button exitButton;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        startButton = uiDocument.rootVisualElement.Q<Button>("Start");
        exitButton = uiDocument.rootVisualElement.Q<Button>("Exit");
        startButton.clicked += StartButtonClicked;
        exitButton.clicked += ExitButtonClicked;
    }

    private void ExitButtonClicked()
    {
        Application.Quit();
    }

    private void StartButtonClicked()
    {
        manager.EnterGame();
    }

    private void OnDisable()
    {
    }


}