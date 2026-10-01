using UnityEngine;
using System.Collections;
//using DG.Tweening;

public class CrossFade : SceneTransition
{
    public CanvasGroup crossFade;

    public override IEnumerator AnimateTransitionIn()
    {
        //var tweener = crossFade.DOFade(1f,1f);
        float transparency = crossFade.alpha;
        float duration = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            crossFade.alpha = Mathf.Lerp(transparency, 1f, elapsed/duration);
            yield return null;
        }
        crossFade.alpha = 1f;
        //yield return tweener.WaitForCompletion();
    }
    public override IEnumerator AnimateTransitionOut()
    {
        //var tweener = crossFade.DOFade(1f,1f);
        float transparency = crossFade.alpha;
        float duration = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            crossFade.alpha = Mathf.Lerp(transparency, 0f, elapsed/duration);
            yield return null;
        }
        crossFade.alpha = 0f;
        //yield return tweener.WaitForCompletion();
    }
}
