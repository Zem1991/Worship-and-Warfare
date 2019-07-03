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
        DBContent defaultContent = GetComponent<DBContent>();
        if (Validate(defaultContent))
        {
            this.defaultContent = defaultContent;
        }

        DBContent[] children = GetComponentsInChildren<DBContent>();
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

    public virtual DBContent Select(int index, bool returnDefault = true)
    {
        if (index >= 0 && index < contents.Count)
            return contents[index];

        if (returnDefault)
            return defaultContent;
        else
            return null;
    }

    public virtual DBContent Select(string id, bool returnDefault = true)
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
