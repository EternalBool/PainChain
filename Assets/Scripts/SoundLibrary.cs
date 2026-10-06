using System;
using UnityEngine;

[Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}
public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;
    public AudioClip GetClipFromName(string name)
    {
        foreach (var soundEff in soundEffects)
        {
            if (soundEff.groupID == name)
            {
                return soundEff.clips[UnityEngine.Random.Range(0, soundEff.clips.Length)];
            }
        }
        return null;
    }
}
