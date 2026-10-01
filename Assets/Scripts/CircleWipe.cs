using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CircleWipe : SceneTransition
{
    public Image circle;
    public override IEnumerator AnimateTransitionIn()
    {
        float elapsed = 0f;
        float duration = 1f;
        Vector2 pos = new Vector2(-1000f, 0f);
        Vector2 targ = new Vector2(0f,0f);
        circle.rectTransform.anchoredPosition = pos;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            circle.rectTransform.anchoredPosition = Vector2.Lerp(pos, targ, elapsed/duration);
            yield return null;
        }
        circle.rectTransform.anchoredPosition = targ;
    }
    public override IEnumerator AnimateTransitionOut()
    {
        float elapsed = 0f;
        float duration = 1f;
        Vector2 pos = new Vector2(0f, 0f);
        Vector2 targ = new Vector2(1000f,0f);
        circle.rectTransform.anchoredPosition = pos;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            circle.rectTransform.anchoredPosition = Vector2.Lerp(pos, targ, elapsed/duration);
            yield return null;
        }
        circle.rectTransform.anchoredPosition = targ;
    }
}
