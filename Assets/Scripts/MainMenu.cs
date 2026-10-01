using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButt;
    [SerializeField] private Button credits;
    private Transform cp;
    private Button credx;
    
    private void Awake()
    {
        cp = credits.transform.Find("CreditPanel");
        credx = cp.transform.Find("X").GetComponent<Button>();
        credits.onClick.AddListener(OpenCred);
        credx.onClick.AddListener(CloseCred);
        playButt.onClick.AddListener(LinkUp);
    }
    private void Start()
    {
        MusicManager.Instance.PlayMusic("Link", 5f, 1f);
    }
    public void LinkUp()
    {
        LevelManager.Instance.LoadScene("PainChain", "CrossFade");
        MusicManager.Instance.PlayMusic("Chain", 5f, 1f);
    }
    private void OpenCred()
    {
        cp.gameObject.SetActive(true);
    }
    private void CloseCred()
    {
        cp.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        credits.onClick.RemoveListener(OpenCred);
        credx.onClick.RemoveListener(CloseCred);
        playButt.onClick.RemoveListener(LinkUp);
    }
}
