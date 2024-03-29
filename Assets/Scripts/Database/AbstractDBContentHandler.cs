﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public virtual List<T> SelectAll()
    {
        return contents.Values.ToList();
    }

    public virtual T Select(string id)
    {
        bool found = contents.TryGetValue(id, out T result);
        if (found) return result;
        return null;
    }

    private bool Validate(T item)
    {
        Type itemType = item.GetType();
        Type handlerType = typeof(T);

        bool correctType = itemType == handlerType || itemType.IsSubclassOf(handlerType);
        if (correctType && VerifyContent(item))
        {
            return true;
        }
        else
        {
            Debug.LogWarning(GetType() + ": Content is not valid! " + item);
            return false;
        }
    }

    protected abstract bool VerifyContent(T item);
}
