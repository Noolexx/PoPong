using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    
    public void Play()
    {
        SceneManager.LoadScene("classic");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
