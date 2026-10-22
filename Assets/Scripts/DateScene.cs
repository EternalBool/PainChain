using UnityEngine;

public class DateScene : SceneController
{
    Animator bruh => links[0];

    protected override void ChainLink(string ent, string act)
    {
        switch (ent)
        {
            case "bruh": bruh.SetTrigger(act); break;
            default: Debug.LogWarning($"Unknown actor '{ent}' in link '{act}'"); break;
        }
    }
}
