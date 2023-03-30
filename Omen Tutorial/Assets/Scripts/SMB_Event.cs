using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Has editor Editor_SMB_Event 
public enum SMBTiming { OnEnter, OnExit, OnUpdate, OnEnd }

public class SMB_Event : StateMachineBehaviour
{
    [SerializeField] private int _totalFrames;
    [SerializeField] private int _currentFrame;
    [SerializeField] private float _normalizedTime;
    [SerializeField] private float _normalizedTimeUncapped;
    [SerializeField] private string _motionTime = "";
    public List<SMBEvent> Events = new List<SMBEvent>();

    private bool _hasParam;
    private Comp_SMBEventCurrator _eventCurrator;

    //This does not work with blend trees

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _hasParam = SMBFunctions.HasParameter(animator, _motionTime);
        _eventCurrator = animator.GetComponent<Comp_SMBEventCurrator>();
        _totalFrames = SMBFunctions.GetTotalFrames(animator, layerIndex);

        _normalizedTimeUncapped = stateInfo.normalizedTime;
        _normalizedTime = _hasParam ? animator.GetFloat(_motionTime) : SMBFunctions.GetNormalizedTime(stateInfo);
        _currentFrame = SMBFunctions.GetCurrentFrame(_totalFrames, _normalizedTime);
        foreach (SMBEvent smbEvent in Events)
        {
            smbEvent.fired = false;
            if (smbEvent.timing == SMBTiming.OnEnter)
            {
                smbEvent.fired = true;
                _eventCurrator.Raise(smbEvent.eventName);
            }
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _normalizedTimeUncapped = stateInfo.normalizedTime;
        _normalizedTime = _hasParam ? animator.GetFloat(_motionTime) : SMBFunctions.GetNormalizedTime(stateInfo);
        _currentFrame = SMBFunctions.GetCurrentFrame(_totalFrames, _normalizedTime);

        if (_eventCurrator)
            foreach (SMBEvent smbEvent in Events)
                if (!smbEvent.fired)
                {
                    if (smbEvent.timing == SMBTiming.OnUpdate)
                    {
                        if (_currentFrame >= smbEvent.onUpdateFrame)
                        {
                            smbEvent.fired = true;
                            _eventCurrator.Raise(smbEvent.eventName);
                        }
                    }
                    else if (smbEvent.timing == SMBTiming.OnEnd)
                    {
                        if (_currentFrame >= _totalFrames)
                        {
                            smbEvent.fired = true;
                            _eventCurrator.Raise(smbEvent.eventName);
                        }
                    }
                }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eventCurrator)
            foreach (SMBEvent smbEvent in Events)
                if (smbEvent.timing == SMBTiming.OnExit)
                {
                    smbEvent.fired = true;                        
                    _eventCurrator.Raise(smbEvent.eventName);
                }                       
    }
}

[System.Serializable]
public class SMBEvent
{
    [Attribute_ReadOnly] public bool fired;
    public string eventName;
    public SMBTiming timing;
    public float onUpdateFrame = 1;        
}
