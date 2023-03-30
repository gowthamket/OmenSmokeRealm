using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    
}
public interface IGameEventListener
{
    void OnEventRaised();
}

public interface IGameEventListener<T>
{
    void OnEventRaised(T t);
}

