using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

[System.Serializable]
public class GameObjectPair
{
    // one is normal version, other is shadow realm version
    public GameObject gameObject1;
    public GameObject gameObject2;
}

public class MeshSwapper : MonoBehaviour
{
    [SerializeField] private bool m_swapped;
    [SerializeField] private List<GameObjectPair> m_gameObjectPairs = new List<GameObjectPair>();

    //public void Swap()
    //{
    //    if (m_swapped)
    //    {
    //        SwapBack();
    //    }
    //    else
    //    {
    //        SwapTo();
    //    }
    //}
    public void SwapTo() // iterates through the pair and disables one and enables the other
    {
        if (!m_swapped)
        {
            m_swapped = true;
            foreach (GameObjectPair pair in m_gameObjectPairs)
            {
                if (pair.gameObject1 != null) { pair.gameObject1.SetActive(false); }
                if (pair.gameObject2 != null) { pair.gameObject2.SetActive(true); }
            }
        }
    }
    public void SwapBack()
    {
        if (m_swapped)
        {
            m_swapped = false;
            foreach (GameObjectPair pair in m_gameObjectPairs)
            {
                if (pair.gameObject1 != null) { pair.gameObject1.SetActive(true); }
                if (pair.gameObject2 != null) { pair.gameObject2.SetActive(false); }
            }
        }
    }
}
