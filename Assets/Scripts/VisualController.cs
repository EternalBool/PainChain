using System;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemp;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 flyDir = new Vector3(0f, -200f, 0f);
    [SerializeField] private float flyOffDur = 0.3f;

    private GameObject currIcon;
    private Coroutine flyOff;
    public void AdvanceKey(char nextDir)
    {
        if (flyOff != null)
        {
            StopCoroutine(flyOff);
            flyOff = null;
        }
        if (currIcon != null)
        {
            flyOff = StartCoroutine(FlyKey(currIcon, nextDir));
        }
        else
        {
            ShowKey(nextDir);
        }
    }
    public void ShowKey(char direction)
    {
        currIcon = Instantiate(buttonTemp, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        ComboButtonDisplay display = currIcon.GetComponent<ComboButtonDisplay>();
        display.setDir(direction);
    }
    private IEnumerator FlyKey(GameObject icon, char nextDir)
    {
        RectTransform rt = icon.GetComponent<RectTransform>();
        Vector3 sp = rt.anchoredPosition;
        Vector3 ep = sp + flyDir;

        if (currIcon != null) currIcon.GetComponent<ComboButtonDisplay>().pressed(); 
        float elapsed = 0f;
        while (elapsed < flyOffDur)
        {
            if (rt == null) yield break;
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector3.Lerp(sp,ep,elapsed/flyOffDur);
            yield return null;
        }
        if (icon != null) Destroy(icon); 
        currIcon = null;
        flyOff = null;
        ShowKey(nextDir);
    }
}
