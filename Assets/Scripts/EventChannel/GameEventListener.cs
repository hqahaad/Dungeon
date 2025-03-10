using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener<T> : MonoBehaviour, IGameEventListener<T>
{
    [SerializeField] private GameEvent<T> gameEvent;
    [SerializeField] private UnityEvent<T> unityEvent;

    void OnEnable() => gameEvent.RegisterListener(this);
    void OnDisable() => gameEvent.DeregisterListener(this);

    public void OnEventRaised(T data) => unityEvent?.Invoke(data);
}

public class GameEventListener : GameEventListener<Unit> { }