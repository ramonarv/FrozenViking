using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    public void LoadMap()
    {
        // We want to run this function when the player presses "play" button in the main menu
        SceneManager.LoadScene("Map");
    }

    public void Save()
    {
        Debug.Log("Save pressed");
        GameManager.manager.Save();

    }

    public void Load()
    {
        Debug.Log("Load pressed");
        GameManager.manager.Load();
    }
}
