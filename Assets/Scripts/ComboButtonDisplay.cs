using UnityEngine;

public class ComboButtonDisplay : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image button;
    [SerializeField] private UnityEngine.UI.Image arrows;
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite right;
    [SerializeField] private Sprite disabled;

    public void setDir(char direction)
    {
        switch (direction)
        {
            case 'U': arrows.sprite = up; break;
            case 'D': arrows.sprite = down; break;
            case 'L': arrows.sprite = left; break;
            case 'R': arrows.sprite = right; break;
        }
    }
    public void pressed()
    {
        button.sprite = disabled;
    }
}
