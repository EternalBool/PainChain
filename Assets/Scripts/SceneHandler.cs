using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private Transform scenesParent;

    private readonly List<GameObject> scenes = new List<GameObject>();
    private int currentIndex = 0;

    void Awake()
    {
        foreach (Transform child in scenesParent)
        {
            scenes.Add(child.gameObject);
        }
    }

    void Start()
    {
        ShowScene(0);
        MusicManager.Instance.PlayMusic("Chain");
    }

    public void NextScene()
    {
        ShowScene((currentIndex + 1) % scenes.Count);
    }
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