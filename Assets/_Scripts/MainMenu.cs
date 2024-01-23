using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button startGameButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;
    public GameObject GameTitle;

    public GameObject settingsPanel;
    void Start()
    {

        startGameButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        backButton.onClick.AddListener(HideSettings);

        settingsPanel.SetActive(false);
    }


    void StartGame()
    {

        SceneManager.LoadScene("Main");
    }


    void OpenSettings()
    {
        
        settingsPanel.SetActive(true);

     
        startGameButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        GameTitle.SetActive(false);

        
    }

    void HideSettings()
    {
      
        settingsPanel.SetActive(false);
        startGameButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        GameTitle.SetActive(true);


    }
    void QuitGame()
    {
       
        Application.Quit();
        Debug.Log("Game Quit"); 
    }
}
