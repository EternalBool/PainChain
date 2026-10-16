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
        public float rotation;
        public float duration;
    }
    protected int currIn = -1;
    Vector3[] startPos; 
    Quaternion[] startRot;
    public virtual void Awake()
    {
        startPos = new Vector3[links.Length];
        startRot = new Quaternion[links.Length];
        for (int i = 0; i < links.Length; i++)
        {
            startPos[i] = links[i].transform.localPosition;
            startRot[i] = links[i].transform.localRotation;
        }
    }
    public virtual void Enter()
    {
        Debug.Log("<color=green>Entering!!!</color>");
        StopAllCoroutines();
        foreach (var link in links)
        {
            if (link == null) continue;
            link.Rebind();
            link.Update(0f);
            /*
            foreach (var p in link.parameters)
            {
                if (p.type == AnimatorControllerParameterType.Trigger)
                {
                    link.ResetTrigger(p.name);
                }
            }*/
        }
        for (int i = 0; i < startPos.Length; i++)
        {
            links[i].transform.localPosition = startPos[i];
            links[i].transform.localRotation = startRot[i];
        }
        
        //ChainLink("tutti", "entry");
        currIn = -1;
    }
    public virtual void Exit()
    {
        Debug.Log("<color=red>Exiting!!!</color>");
        StopAllCoroutines();
        currIn = -1;
    }
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
            StartCoroutine(Motion(step.link, step.destination, step.rotation, step.duration));
        }
    }

    IEnumerator Motion(Transform link, Vector3 destination, float rotation, float duration)
    {
        Vector3 start = link.localPosition;
        Quaternion startRot = link.localRotation;
        for (float e = 0f; e < duration; e += Time.deltaTime)
        {
            link.localPosition = Vector3.Lerp(start, destination, Mathf.SmoothStep(0f,1f,e/duration));
            if (rotation != 0f)
            {
                float currentAngle = Mathf.Lerp(0f, rotation, e/duration);
                link.localRotation = startRot * Quaternion.Euler(0f,0f,currentAngle);
            }
            yield return null;
        }
        link.localPosition = destination;
    }

    protected abstract void ChainLink(string ent, string act);
}