using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWipe : MonoBehaviour
{
    [SerializeField] private float m_obscureTime;
    [SerializeField] private Animator m_aniamtor;

    private bool m_fired;
    private List<IGameEventListener> m_listeners = new List<IGameEventListener>();

    private void Update()
    {
        if (!m_fired)
        {
            AnimatorStateInfo stateInfo = m_aniamtor.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Base Layer.Wipe Anim"))
            {
                if (stateInfo.normalizedTime >= m_obscureTime)
                {
                    m_fired = true;
                    Raise();
                }
            }
        }
    }
    public void StartWipe()
    {
        m_fired = false;
        m_aniamtor.CrossFade("Wipe Anim", 0); //cross-fades animation without a transition duration
    }
    private void Raise()
    {
        for (int i = m_listeners.Count - 1; i >= 0; i--)
            m_listeners[i].OnEventRaised();
    }
    public void RegisterListener(IGameEventListener listener)
    {
        if (!m_listeners.Contains(listener))
            m_listeners.Add(listener);
    }
    public void UnregisterListener(IGameEventListener listener)
    {        
        m_listeners.Remove(listener);
    }



}
