using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunBehaviourOnEnable : MonoBehaviour
{


    public UnityEvent EventToHappen;

    void OnEnable()
    {
        
            EventToHappen.Invoke();
        
    }

}
