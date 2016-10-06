using UnityEngine;
using UnityEngine.UI;

public class PopUpTextScript : IPoolObject
{
    public GameObject floatingText;
    Animator animator = null;
    Text uiText;

    public override void Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        base.Spawn(position, rotation, scale);
        if (!animator)
        {
            animator = floatingText.GetComponent<Animator>();
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            secondsForDeath = clipInfo[0].clip.length;
        }
        base.Destroy();
    }

    public void SetText(string msg)
    {
        if (animator) {
            uiText = animator.GetComponent<Text>();
            uiText.text = msg;
        }
    }

}
