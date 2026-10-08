using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private Transform scenesParent;

    private readonly List<GameObject> scenes = new List<GameObject>();
    private SceneController current;
    private int currentIndex = -1;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            scenes.Add(child.gameObject);
        }
    }

    void Start() => ShowScene(0); //MusicManager.Instance.PlayMusic("Chain");
    public void Advance()
    {
        if (current != null) current.Advance();
    }

    public void NextScene() => ShowScene((currentIndex + 1) % scenes.Count);
    public void ShowScene(string sceneName)
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].name == sceneName)
            {
                ShowScene(i);
                return;
            }
        }
        Debug.LogWarning($"No scene named '{sceneName}' under {scenesParent.name}");
    }

    void ShowScene(int index)
    {
        if (index == currentIndex) return;
        if (current != null) current.Exit();
        currentIndex = index;
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes[i].SetActive(i == currentIndex);
        }
        scenes[currentIndex].TryGetComponent(out current);
        if (current != null) current.Enter();
    }
}