using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DBContentHandler : MonoBehaviour
{
    private List<DBContent> contents = new List<DBContent>();
    public DBContent defaultContent;

    void Awake()
    {
        DBContent defaultValue = GetComponent<DBContent>();
        if (Validate(defaultValue))
        {
            this.defaultContent = defaultValue;
        }

        DBContent[] children = GetComponentsInChildren<DBContent>();
        foreach (var item in children)
        {
            if (item.gameObject != defaultValue.gameObject)
            {
                if (Validate(item))
                {
                    contents.Add(item);
                }
            }
        }
    }

    public virtual DBContent Select(int id)
    {
        if (id >= 0 && id < contents.Count)
            return contents[id];
        else
            return defaultContent;
    }

    private bool Validate(DBContent item)
    {
        bool correctType = item.GetType() == ContentType();
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

    protected abstract Type ContentType();

    protected abstract bool VerifyContent(DBContent item);
}
