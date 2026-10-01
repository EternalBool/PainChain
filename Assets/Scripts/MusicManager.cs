using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayMusic(string trackName, float fadeDur = 0.5f, float wait = 0f)
    {
        //Debug.Log($"Music Fade Dur: {fadeDur}");
        StartCoroutine(AnimateMusicCrossFade(musicLibrary.GetClipFromName(trackName), fadeDur, wait));
    }
    IEnumerator AnimateMusicCrossFade(AudioClip nextTrack, float fadeDur, float wait)
    {
        Debug.Log($"Wait for {wait}s");
        if (wait > 0) yield return new WaitForSeconds(wait);
        float percent = 0;
        while (percent > 1)
        {
            percent += Time.deltaTime * 1/fadeDur;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }
        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1/fadeDur;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
    public void BreakMusic(string trackName, float fadeDur = 2.5f, float pitch = 0.5f)
    {
        StartCoroutine(AnimateMusicChop(musicLibrary.GetClipFromName(trackName), fadeDur, pitch));
    }
    IEnumerator AnimateMusicChop(AudioClip currTrack, float fadeDur, float pitch)
    {
        if (musicSource.clip == currTrack)
        {
            Debug.Log("Slowing Track");
            float elapsed = 0f;
            float current = musicSource.pitch;
            while (elapsed < fadeDur)
            {
                elapsed += Time.deltaTime;
                musicSource.pitch = Mathf.Lerp(current, pitch, elapsed/fadeDur);
                yield return null;
            }
            musicSource.pitch = pitch;
            musicSource.Stop();
            musicSource.pitch = 1f;
        }
        else
        {
            Debug.Log("Wrong Track Homie!");
        }
    }
}
