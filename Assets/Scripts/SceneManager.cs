using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class SceneCounter : MonoBehaviour
{


    public void ChangeCurrentScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene + 1);

        Debug.Log("On to the next scene");
    }

    public void CanvasVisibility(Canvas canvas, bool isEnabled)
    {
        canvas.enabled = isEnabled;
    }

}
