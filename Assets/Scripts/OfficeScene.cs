using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class OfficeScene : SceneController
{
    Animator boss => links[0];
    Animator emp => links[1];

    protected override void ChainLink(string ent, string act)
    {
       switch (ent)
        {
            case "boss": boss.SetTrigger(act); break;
            case "emp": emp.SetTrigger(act); break;
            case "tutti": boss.SetTrigger(act); emp.SetTrigger(act); break;
            default: Debug.LogWarning($"Unknown actor '{ent}' in link '{act}'"); break;
        } 
    }
}
