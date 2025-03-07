using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayableGraph playableGraph;
    private AnimationPlayableOutput playableOutput;
    private AnimationMixerPlayable mixer;
    private AnimationLayerMixerPlayable layerMixer;

    [Header("Test")]
    public AnimationClip clip1;
    public AnimationClip clip2;

    [SerializeField] private List<AvatarMask> masks = new();
    Dictionary<AvatarMask, AnimationLayerMixerPlayable> keyValuePairs = new();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayAnimation(clip1, 1);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayAnimation(clip2, 0);
        }
    }

    void Awake()
    {
        playableGraph = PlayableGraph.Create("AnimationGraph");
        playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);

        mixer = AnimationMixerPlayable.Create(playableGraph, 2);
        playableOutput.SetSourcePlayable(mixer);

        playableGraph.Play();
    }

    public void PlayAnimation(AnimationClip clip, int index)
    {
        var playable = AnimationClipPlayable.Create(playableGraph, clip);
        playableGraph.Connect(playable, 0, mixer, index);
        mixer.SetInputWeight(index, 1f);
    }

    void OnDestroy()
    {
        playableGraph.Destroy();
    }
}
