using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private Button startButton;
    private Button exitButton;

    public ButtonAudio buttonAudio;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        buttonAudio.AddButtonSounds(uiDocument);
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
        EnterGame();
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("SimplePoly City - Low Poly Assets_Demo Scene");
        Debug.Log("Sport");
    }

}