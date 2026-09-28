using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Transform scenesParent; // the "Scenes" object in your hierarchy

    private readonly List<GameObject> scenes = new List<GameObject>();
    private int currentIndex = 0;

    void Awake()
    {
        // Collect every child of "Scenes" in hierarchy order
        foreach (Transform child in scenesParent)
        {
            scenes.Add(child.gameObject);
        }
    }

    void Start()
    {
        ShowScene(0);
    }

    // Advance to the next scene, looping back to the first
    public void NextScene()
    {
        ShowScene((currentIndex + 1) % scenes.Count);
    }

    // Jump to a scene by its GameObject name, e.g. "STREET"
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
        currentIndex = index;
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes[i].SetActive(i == currentIndex);
        }
    }
}