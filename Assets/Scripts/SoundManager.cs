using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private SoundLibrary sfxLibrary;
    [SerializeField] private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    } 
    public void PlaySound3D(AudioClip clip, Vector3 pos, float vol = 0.7f)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, vol);
        }
    }
    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }
    public void PlaySound2D(string soundName, float vol = 0.5f)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName), vol);
    }
}