using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Singleton;


    [Header("Scene Settings")]
    [SerializeField] private int _gameScene;
    [SerializeField] private int _menuScene;
    
    [Header("Game Over Settings")]
    [SerializeField] private GameObject _gameOverObject;
    [SerializeField] private Text _gameOverScore;

    [Header("Main Menu Stuff")]
    [SerializeField] private GameObject _menuHolder;
    [SerializeField] private GameObject _infoHolder;
    
    private void Start()
    {
        if (Singleton != null)
        {
            Destroy(this);
            return;
        }

        Singleton = this;
    }

    public void EnableGameOverMenu()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        _gameOverObject.SetActive(true);
        _gameOverScore.text = Player.Singleton.PlayerScore + "";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        SceneManager.UnloadSceneAsync(_gameScene);
        SceneManager.LoadScene(_gameScene);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        SceneManager.UnloadSceneAsync(_gameScene);
        SceneManager.LoadScene(_menuScene);
    }

    public void ToGameScene()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        SceneManager.UnloadSceneAsync(_menuScene);
        SceneManager.LoadScene(_gameScene);
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void ToggleGameInfo(bool toggleInfo)
    {
        _menuHolder.SetActive(!toggleInfo);
        _infoHolder.SetActive(toggleInfo);
    }
}