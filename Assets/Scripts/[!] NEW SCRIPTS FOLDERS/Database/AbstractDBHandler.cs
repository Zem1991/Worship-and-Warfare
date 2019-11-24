using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDBContentHandler<T> : AbstractSingleton<AbstractDBContentHandler<T>> where T : AbstractDBContent
{
    private Dictionary<string, T> contents = new Dictionary<string, T>();

    public override void Awake()
    {
        base.Awake();

        T[] children = GetComponentsInChildren<T>();
        foreach (var item in children)
        {
            if (Validate(item))
            {
                contents.Add(item.id, item);
            }
        }
    }

    public virtual T Select(string id)
    {
        bool result = contents.TryGetValue(id, out T content);
        if (result) return content;
        return null;
    }

    private bool Validate(T item)
    {
        bool correctType = item.GetType() == typeof(T);
        if (correctType && VerifyContent(item))
        {
            return true;
        }
        else
        {
            Debug.LogWarning("Content is not valid! " + item);
            return false;
        }
    }

    protected abstract bool VerifyContent(T item);
}
