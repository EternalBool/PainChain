using System;
using System.Collections;
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

    public void ShowKey(char direction)
    {
        currIcon = Instantiate(buttonTemp, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        ComboButtonDisplay display = currIcon.GetComponent<ComboButtonDisplay>();
        display.setDir(direction);
    }
    public void AdvanceKey(char nextDir)
    {
        if (currIcon != null)
        {
            StartCoroutine(FlyKey(currIcon, nextDir));
        }
        else
        {
            ShowKey(nextDir);
        }
    }
    private IEnumerator FlyKey(GameObject icon, char nextDir)
    {
        RectTransform rt = icon.GetComponent<RectTransform>();
        Vector3 sp = rt.anchoredPosition;
        Vector3 ep = sp + flyDir;

        float elapsed = 0f;
        while (elapsed < flyOffDur)
        {
            if (rt == null) yield break;
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector3.Lerp(sp,ep,elapsed/flyOffDur);
            yield return null;
        }
        if (icon != null)
        {
           Destroy(icon); 
        }
        ShowKey(nextDir);
    }
}
