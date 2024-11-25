using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Change to a specific scene using its build index
    public void ChangeSceneTo(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("Invalid scene index: " + sceneIndex);
        }
    }

    // Restart the current scene
    public void RestartCurrentLevel()
    {
        print("Restart");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Ensure the cursor is unlocked and visible
    public void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;                 // Make the cursor visible
    }

    public void FindAndDisablePlayer()
    {
        GameObject.FindWithTag("Player").SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        print("Paused Game");
    }

}
