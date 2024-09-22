using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{

    private VisualElement mainUI;

    private Button playButton;

    private Button settingsButton;

    private Button exitButton;
    private VisualElement settingsMenu;
    private VisualElement mainMenu;

    private void Awake()
    {

        mainUI = GetComponent<UIDocument>().rootVisualElement;
        settingsMenu = mainUI.Q<VisualElement>("SettingsMenu");
        mainMenu = mainUI.Q<VisualElement>("MenuBackground");

    }
    private void OnEnable()
    {
        playButton = mainUI.Q<Button>("PlayButton");
        playButton.clicked += PlayPressed;

        exitButton = mainUI.Q<Button>("ExitButton");
        exitButton.clicked += ExitPressed;

        settingsButton = mainUI.Q<Button>("SettingsButton");
        settingsButton.clicked += SettingsPressed;
    }
    private void OnDisable()
    {
        playButton.clicked -= PlayPressed;
        exitButton.clicked -= ExitPressed;
        settingsButton.clicked -= SettingsPressed;
    }

    private void PlayPressed()
    {
        print("Play pressed");
        SceneManager.LoadScene("Scene1");
    }
    private void ExitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif


    }

    private void SettingsPressed()
    {

        mainMenu.style.display = DisplayStyle.None;
        settingsMenu.style.display = DisplayStyle.Flex;
    }
}
