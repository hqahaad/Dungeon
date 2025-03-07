using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AvatarMask upperBodyMask;
    [SerializeField] private AvatarMask lowerBodyMask;

    public Transform target;

    [ContextMenu("asd")]
    private void ChangeLeg()
    {
        var c = animator.runtimeAnimatorController as AnimatorController;
        c.layers[0].avatarMask = upperBodyMask;

        Debug.Log("111");
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        //animator.SetIKPosition(AvatarIKGoal.LeftHand, target.transform.position);
    }
}
