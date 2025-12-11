using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenUI;

    public void ShowDeathScreen()
    {
        deathScreenUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        PlayerStats.isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
