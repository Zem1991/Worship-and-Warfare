using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DBContentHandler<T> : MonoBehaviour where T : DBContent
{
    private List<T> contents = new List<T>();
    public T defaultContent;

    void Awake()
    {
        T defaultContent = GetComponent<T>();
        if (Validate(defaultContent))
        {
            this.defaultContent = defaultContent;
        }

        T[] children = GetComponentsInChildren<T>();
        foreach (var item in children)
        {
            if (item.gameObject != defaultContent.gameObject)
            {
                if (Validate(item))
                {
                    contents.Add(item);
                }
            }
        }
    }

    public virtual T Select(int index, bool returnDefault = true)
    {
        index--;
        if (index >= 0 && index < contents.Count)
            return contents[index];

        if (returnDefault)
            return defaultContent;
        else
            return null;
    }

    public virtual T Select(string id, bool returnDefault = true)
    {
        foreach (var item in contents)
        {
            if (item.id.Equals(id))
                return item;
        }

        if (returnDefault)
            return defaultContent;
        else
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
