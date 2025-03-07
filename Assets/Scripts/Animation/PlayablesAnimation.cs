using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayablesAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayableGraph playableGraph;
    private AnimationPlayable animationPlayable;

    public AnimationClip clipTest1;
    public AnimationClip clipTest2;

    public AvatarMask upperMask;
    public AvatarMask lowerMask;

    void Awake()
    {
        playableGraph = PlayableGraph.Create($"{gameObject.name} - Graph");

        animationPlayable = new(playableGraph, animator);
        animationPlayable.Initialize(3);

        playableGraph.Play();

        animationPlayable.SetLayerAvatarMask(1, upperMask);
        animationPlayable.SetLayerAvatarMask(2, lowerMask);

        animationPlayable.SetLayerWeight(1, 0f);
        animationPlayable.SetLayerWeight(2, 0f);

        animationPlayable.Play(clipTest1, 1);
        animationPlayable.Play(clipTest1, 2);
        animationPlayable.Play(clipTest2, 0);
    }
}





public class AnimationPlayable
{
    private readonly PlayableGraph graph;
    private readonly Animator animator;

    private AnimationLayerMixerPlayable layerMixer;
    private List<MixerInstance> mixers = new();

    public AnimationPlayable(PlayableGraph graph, Animator animator)
    {
        this.graph = graph;
        this.animator = animator;
    }

    public void Initialize(int layerCount)
    {
        var playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);

        layerMixer = AnimationLayerMixerPlayable.Create(graph, layerCount);

        for (int i = 0; i < layerCount; i++)
        {
            var mixerInstance = new MixerInstance(graph);

            mixers.Add(mixerInstance);

            layerMixer.ConnectInput(i, mixerInstance.rootMixer, 0);

            SetLayerWeight(i, 1f);
            SetLayerAddtiveMode(i, false);
        }

        playableOutput.SetSourcePlayable(layerMixer);
    }

    public void SetLayerWeight(int layerIndex, float weight)
    {
        if (!layerMixer.IsValid())
            return;

        layerMixer.SetInputWeight(layerIndex, weight);
    }

    public void SetLayerAvatarMask(int layerIndex, AvatarMask mask)
    {
        if (!layerMixer.IsValid() || mask == null)
            return;

        layerMixer.SetLayerMaskFromAvatarMask((uint)layerIndex, mask);
    }

    public void SetLayerAddtiveMode(int layerIndex, bool addtive)
    {
        if (!layerMixer.IsValid())
            return;

        layerMixer.SetLayerAdditive((uint)layerIndex, addtive);
    }

    public void Play(AnimationClip clip, int layerIndex = 0)
    {
        mixers[layerIndex].PlayAnimationClip(clip);
    }

    public void PlayOneShot(AnimationClip clip, int layerIndex = 0)
    {
        mixers[layerIndex].PlayOnShotAnimationClip(clip);
    }
}

public class MixerInstance
{
    private readonly PlayableGraph graph;

    public AnimationMixerPlayable rootMixer;

    public AnimationMixerPlayable motionMixer;
    public AnimationMixerPlayable oneShotMixer;

    private bool isOneShotPlaying = false;

    public MixerInstance(PlayableGraph graph)
    {
        this.graph = graph;

        rootMixer = AnimationMixerPlayable.Create(graph, 2);
        motionMixer = AnimationMixerPlayable.Create(graph, 2);
        oneShotMixer = AnimationMixerPlayable.Create(graph, 2);

        if (!rootMixer.IsValid() || !motionMixer.IsValid() || !oneShotMixer.IsValid())
        {
            Debug.LogError("MixerInstance: One or more AnimationMixerPlayable instances are invalid!");
            return;
        }

        //rootMixer.ConnectInput(0, motionMixer, 0);
        //rootMixer.ConnectInput(1, oneShotMixer, 1);

        graph.Connect(motionMixer, 0, rootMixer, 0);
        graph.Connect(oneShotMixer, 0, rootMixer, 1);

        rootMixer.SetInputWeight(0, 1f);
        rootMixer.SetInputWeight(1, 0f);

        motionMixer.SetInputWeight(0, 1f);
        motionMixer.SetInputWeight(1, 0f);

        oneShotMixer.SetInputWeight(0, 1f);
        oneShotMixer.SetInputWeight(1, 0f);
    }

    public void PlayAnimationClip(AnimationClip clip)
    {
        AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(graph, clip);
        AnimationClipPlayable clipPlayable2 = AnimationClipPlayable.Create(graph, clip);
        motionMixer.ConnectInput(0, clipPlayable, 0);
        motionMixer.ConnectInput(1, clipPlayable2, 0);

        motionMixer.SetInputWeight(0, 1f);
        motionMixer.SetInputWeight(1, 0f);
    }

    public void PlayOnShotAnimationClip(AnimationClip clip)
    {
        oneShotMixer.SetInputWeight(0, 0f);
        oneShotMixer.SetInputWeight(1, 1f);
    }
}
