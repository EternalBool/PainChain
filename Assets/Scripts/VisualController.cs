using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using Unity.Collections;

public class VisualController : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemp;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float flyOffDur = 1f;
    [SerializeField] private float dist = 1000f;
    [SerializeField, Range(0.5f, 15f)] private float spinSpeed = 1f;
    private Dictionary<char, (Vector3 dir, float rot)> flyDir;

    private GameObject currIcon;
    private Coroutine flyOff;
    private void Awake()
    {
        flyDir = new Dictionary<char, (Vector3 dir, float rot)>
        {
            {'U', (new Vector3(0,dist, 0f), 360f)},
            {'D', (new Vector3(0,-dist, 0f), -360f)},
            {'L', (new Vector3(-dist,0f, 0f), -360f)},
            {'R', (new Vector3(dist,0f, 0f), 360f)},
        };
    }
    public void AdvanceKey(char nextDir, char? currDir = null)
    {
        
        /* if (flyOff != null)
        {
            StopCoroutine(flyOff);
            flyOff = null;
        } */
        if (currIcon != null)
        {
            // flyOff = StartCoroutine(FlyKey(currIcon, nextDir, currDir.Value));
            StartCoroutine(FlyKey(currIcon, nextDir, currDir.Value));
        }
    }
    public void ShowKey(char direction, float delay = 0f) => StartCoroutine(KeyShow(direction, delay));
    private IEnumerator KeyShow(char direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        //Debug.LogWarning("SHOWKEY");
        currIcon = Instantiate(buttonTemp, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        ComboButtonDisplay display = currIcon.GetComponent<ComboButtonDisplay>();
        display.setDir(direction);
    }
    private IEnumerator FlyKey(GameObject icon, char nextDir, char currDir)
    {
        //Debug.LogWarning("ADVANCEKEY");
        
        RectTransform rt = icon.GetComponent<RectTransform>();
        Vector3 sp = rt.anchoredPosition;
        Vector3 ep = sp + flyDir[currDir].dir;
        Quaternion rp = rt.localRotation;
        Image ii = icon.GetComponent<Image>();
        Image ai = icon.transform.Find("Arrows").GetComponent<Image>();


        if (currIcon != null) currIcon.GetComponent<ComboButtonDisplay>().pressed();
        if (nextDir != 'X') StartCoroutine(KeyShow(nextDir, 0.2f));
        for (float e = 0f; e < flyOffDur; e += Time.deltaTime)
        {
            if (rt == null) yield break;
            ii.color = Color.Lerp(ii.color, Color.clear, e/flyOffDur);
            ai.color = Color.Lerp(ai.color, Color.clear, e/flyOffDur);
            rt.anchoredPosition = Vector3.Lerp(sp,ep,e/flyOffDur);
            float currAng = Mathf.Lerp(0f, flyDir[currDir].rot * spinSpeed, e/flyOffDur);
            rt.localRotation = rp * Quaternion.Euler(0f,0f,currAng);
            yield return null;
        }
        yield return new WaitForSeconds(flyOffDur);
        DestroyKey(icon);
    }
    public void DestroyKey(GameObject icon)
    {
        if (icon == currIcon) currIcon = null;
        if (icon != null) Destroy(icon); 
        //flyOff = null;
    }
    public IEnumerator DieKey()
    {
        Debug.LogWarning("DIEKEY");
        if (currIcon == null) yield break;
        GameObject icon = currIcon;
        RectTransform rt = icon.GetComponent<RectTransform>();
        Image ii = icon.GetComponent<Image>();
        ii.color = Color.red;
        Color sc = ii.color;
        Vector2 ss = rt.localScale;
        for (float e = 0f; e < flyOffDur; e += Time.deltaTime)
        {
            if (icon == null) yield break;
            ii.color = Color.Lerp(sc, Color.clear, e/flyOffDur);
            rt.localScale = Vector2.Lerp(ss, Vector2.zero, e/flyOffDur);
            yield return null;
        }
        Destroy(currIcon);
    }
}
