using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DBContentHandler : MonoBehaviour
{
    public DBContent defaultValue;
    public List<DBContent> content = new List<DBContent>();

    void Awake()
    {
        DBContent defaultValue = GetComponent<DBContent>();
        if (Validate(defaultValue))
        {
            this.defaultValue = defaultValue;
        }

        DBContent[] children = GetComponentsInChildren<DBContent>();
        foreach (var item in children)
        {
            if (item.gameObject != defaultValue.gameObject)
            {
                if (Validate(item))
                {
                    content.Add(item);
                }
            }
        }
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
