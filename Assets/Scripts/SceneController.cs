using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class SceneController : MonoBehaviour
{
    [SerializeField] protected Animator[] links;
    [SerializeField] protected List<SceneStep> sequence;
    [Serializable] public struct SceneStep
    {
        public string action;
        public Transform link;
        public Vector3 destination;
        public float duration;
    }
    protected int currIn = -1;
    Vector3[] startPos; 
    public virtual void Awake()
    {
        startPos = new Vector3[links.Length];
        for (int i = 0; i < links.Length; i++)
        {
            startPos[i] = links[i].transform.localPosition;
        }
    }
    public virtual void Enter()
    {
        for (int i = 0; i < startPos.Length; i++)
        {
            links[i].transform.localPosition = startPos[i];
        }
        foreach (var link in links)
        {
            if (link == null) continue;
            foreach (var p in link.parameters)
            {
                if (p.type == AnimatorControllerParameterType.Trigger)
                {
                    link.ResetTrigger(p.name);
                }
            }
        }
        currIn = -1;
    }
    public virtual void Exit() { }
    public virtual void Advance()
    {
        if (sequence == null || sequence.Count == 0) return;
        currIn = (currIn + 1) % sequence.Count;
        SceneStep step = sequence[currIn];
        string[] parts = step.action.Split('_');
        string ent = parts[0];
        string act = parts[1];
        ChainLink(ent, act);
        if (step.link != null) 
        {
            StartCoroutine(MoveTo(step.link, step.destination, step.duration));
        }
    }

    IEnumerator MoveTo(Transform link, Vector3 destination, float duration)
    {
        Vector3 start = link.localPosition;
        for (float e = 0f; e < duration; e+= Time.deltaTime)
        {
            link.localPosition = Vector3.Lerp(start, destination, Mathf.SmoothStep(0f,1f,e/duration));
            yield return null;
        }
        link.localPosition = destination;
    }

    protected abstract void ChainLink(string ent, string act);
}