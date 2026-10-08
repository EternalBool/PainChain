using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SceneController : MonoBehaviour
{
    [SerializeField] protected Animator[] links;
    [SerializeField] protected List<string> sequence;
    protected int currIn = -1;
    public virtual void Enter()
    {
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
        string[] parts = sequence[currIn].Split('_');
        string ent = parts[0];
        string act = parts[1];
        ChainLink(ent, act);
    }

    protected abstract void ChainLink(string ent, string act);
}