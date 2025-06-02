using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : BaseScreen
{
    //Checkpoins
    //options
    //restart
    //quit

    [SerializeField] BaseScreen _optionsScreen;

    public void CallCheckpoint()
    {
        EventManager.Trigger("CallMementoLoad");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            Application.ExternalEval("window.close();");
#elif UNITY_STANDALONE
            Application.Quit();
#else
            Application.Quit();
#endif

    }

    public void OpenOptions()
    {
        ScreenManager.Instance.ActivateScreen(_optionsScreen);
    }
}
