using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasAppear canvasAppear;
    [SerializeField] Button button;
    [SerializeField] GameObject panel;
    [SerializeField] bool canAppear = false;


    public void EnableVisibilityEvent()
    {
        print("Click credit button!");
        canAppear = !canAppear;

        print(canAppear);
        canvasAppear.SendEvent(panel, canAppear);

    }
}
