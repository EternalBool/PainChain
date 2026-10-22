using UnityEngine;

public class StreetScene : SceneController
{
    Animator whip => links[0];
    protected override void ChainLink(string ent, string act)
    {
        switch (ent)
        {
            case "whip": whip.SetTrigger(act); break;
            default: Debug.LogWarning($"Unknown actor '{ent}' in link '{act}'"); break;
        } 
    }
}
