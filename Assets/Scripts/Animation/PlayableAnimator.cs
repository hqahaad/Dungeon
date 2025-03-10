using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayableGraph playableGraph;
    private PlayableAnimation animationPlayable;

    public AvatarMask upperMask;
    public AvatarMask lowerMask;

    void Awake()
    {
        //юс╫ц
        playableGraph = PlayableGraph.Create($"{gameObject.name} - Graph");

        animationPlayable = new(playableGraph, animator);
        animationPlayable.Initialize(layerCount: 3);

        playableGraph.Play();

        animationPlayable.SetLayerAvatarMask(1, upperMask);
        animationPlayable.SetLayerAvatarMask(2, lowerMask);

        animationPlayable.SetLayerWeight(1, 0f);
        animationPlayable.SetLayerWeight(2, 0f);
    }

    void OnDestroy()
    {
        playableGraph.Destroy();
    }

    public void Play(AnimationClip clip, float duration = 0f, float blendRatio = 1f, int layerIndex = 0)
    {
        if (clip == null)
            return;

        animationPlayable.Play(clip, duration, blendRatio, layerIndex);
    }
}