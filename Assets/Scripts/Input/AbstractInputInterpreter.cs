using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInputInterpreter<T> : MonoBehaviour where T : AbstractInputListener
{
    protected T listener;

    public virtual void Awake()
    {
        listener = GetComponent<T>();
    }
}
