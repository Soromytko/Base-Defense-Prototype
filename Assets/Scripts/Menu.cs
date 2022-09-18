using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject _memu;

    public void SetPause(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        _memu.SetActive(pause);
    }

    public void Restart()
    {
        SetPause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Awake()
    {
        FindObjectOfType<Player>().DieHandler += OnPlayerDie;
    }

    private void OnPlayerDie()
    {
        SetPause(true);
    }
}
