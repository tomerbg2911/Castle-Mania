using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPivot : MonoBehaviour
{
    private void DisableChildOnAnimation(int childNum)
    {
        transform.GetChild(childNum).gameObject.SetActive(false);
    }

    private void EnableChildOnAnimation(int childNum)
    {
        transform.GetChild(childNum).gameObject.SetActive(true);
    }
}
