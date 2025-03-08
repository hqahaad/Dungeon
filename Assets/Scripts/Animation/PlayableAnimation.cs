using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableAnimation
{
    private readonly PlayableGraph graph;
    private readonly Animator animator;

    private AnimationLayerMixerPlayable layerMixer;
    private List<MixerNode> mixers = new();

    public PlayableAnimation(PlayableGraph graph, Animator animator)
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
            var mixerInstance = new MixerNode(graph);

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

    public void Play(AnimationClip clip, float duration = 0f, float blendRatio = 1f, int layerIndex = 0)
    {
        mixers[layerIndex].PlayAnimationClip(clip, duration, blendRatio);
    }

    public void PlayOneShot(AnimationClip clip, int layerIndex = 0)
    {
        //mixers[layerIndex].PlayOnShotAnimationClip(clip);
    }
}

public class MixerNode
{
    private readonly PlayableGraph graph;

    public AnimationMixerPlayable rootMixer;
    public AnimationMixerPlayable motionMixer;
    public AnimationMixerPlayable oneShotMixer;

    private bool isOneShotPlaying = false;
    private readonly List<Playable> remover = new();
    private Coroutine blendHandler;

    private bool isMotionBlending = false;
    private bool isOneShotBlending = false;

    public MixerNode(PlayableGraph graph)
    {
        this.graph = graph;

        rootMixer = AnimationMixerPlayable.Create(graph, 2);
        motionMixer = AnimationMixerPlayable.Create(graph, 2);
        oneShotMixer = AnimationMixerPlayable.Create(graph, 2);

        rootMixer.ConnectInput(0, motionMixer, 0);
        rootMixer.ConnectInput(1, oneShotMixer, 0);

        rootMixer.SetInputWeight(0, 1f);
        rootMixer.SetInputWeight(1, 0f);

        motionMixer.SetInputWeight(0, 1f);
        motionMixer.SetInputWeight(1, 0f);

        oneShotMixer.SetInputWeight(0, 1f);
        oneShotMixer.SetInputWeight(1, 0f);
    }

    public void PlayAnimationClip(AnimationClip clip, float duration = 0f, float blendRatio = 1f)
    {
        if (motionMixer.GetInput(0).IsValid())
        {
            // motionMixer.IsPlayableOfType<T>
            if (motionMixer.GetInput(1).IsValid())
            {
                remover.Add(motionMixer.GetInput(1));
                motionMixer.DisconnectInput(1);
            }

            var oldClip = (AnimationClipPlayable)motionMixer.GetInput(0);
            motionMixer.DisconnectInput(0);
            motionMixer.ConnectInput(1, oldClip, 0);

            var newClip = AnimationClipPlayable.Create(graph, clip);
            motionMixer.ConnectInput(0, newClip, 0);

            //Animation Blending
            if (duration == 0f)
            {
                motionMixer.SetInputWeight(0, blendRatio);
                motionMixer.SetInputWeight(1, 1f - blendRatio);
            }
            else
            {
                //blend start
                if (isMotionBlending)
                {
                    CoroutineGlobal.Handle.StopCoroutine(blendHandler);
                }
                blendHandler = CoroutineGlobal.Handle.StartCoroutine(MotionBlending(duration, blendRatio));
            }
        }
        else
        {
            AnimationClipPlayable newClip = AnimationClipPlayable.Create(graph, clip);

            motionMixer.ConnectInput(0, newClip, 0, 1f);
            motionMixer.SetInputWeight(1, 0f); //extension
        }

        RemoveAnimationsFromMemory();
    }

    private IEnumerator MotionBlending(float duration, float blendRatio)
    {
        isMotionBlending = true;

        motionMixer.SetInputWeight(0, 0f);
        motionMixer.SetInputWeight(1, 1f);

        float blendTime = 0f;
        float amount = 0f;

        while (blendTime < duration)
        {
            blendTime += Time.deltaTime;
            amount += (blendRatio / duration) * Time.deltaTime;

            motionMixer.SetInputWeight(0, amount);
            motionMixer.SetInputWeight(1, 1f - amount);

            yield return null;
        }

        motionMixer.SetInputWeight(0, blendRatio);
        motionMixer.SetInputWeight(1, 1f - blendRatio);

        isMotionBlending = false;
    }

    private void RemoveAnimationsFromMemory()
    {
        foreach (var iter in remover)
        {
            iter.Destroy();
        }

        remover.Clear();
    }

    public void PlayOnShotAnimationClip(AnimationClip clip)
    {
        oneShotMixer.SetInputWeight(0, 0f);
        oneShotMixer.SetInputWeight(1, 1f);
    }
}
