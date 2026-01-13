using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Btn_FX play_btnFX;

    public void OnPlayCliked()
    {
        play_btnFX.Clicked(ChangeScene);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("classic");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
