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
    [SerializeField] public Text GameOverScore;
    [SerializeField] public Text GameOverMessage;
    [SerializeField] public Text GameOverText;

    [Header("Main Menu Stuff")]
    [SerializeField] private GameObject _menuHolder;
    [SerializeField] private GameObject _infoHolder;
    [SerializeField] private GameObject _levelSelectHolder;
    
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
        GameOverScore.text = Player.Singleton.PlayerScore + "";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        var scene = SceneManager.GetActiveScene();
        Cursor.visible = false;
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
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

    public void ToggleLevelSelect(bool toggleInfo)
    {
        _menuHolder.SetActive(!toggleInfo);
        _levelSelectHolder.SetActive(toggleInfo);
    }

    public void LoadLevel(int level)
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        var sceneName = "Level_" + level;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(sceneName);
    }
}