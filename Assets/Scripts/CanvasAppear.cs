using System;
using UnityEngine;

public class CanvasAppear : MonoBehaviour
{

    public event Action UIVisiblity; 


    public void SendEvent(GameObject panel, bool isEnabled)
    {
        panel.SetActive(isEnabled);

        UIVisiblity?.Invoke();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
