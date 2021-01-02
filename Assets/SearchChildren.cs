using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchChildren : MonoBehaviour
{
    public string searchTag;
    [Space]
    public List<GameObject> allChildren;
    public List<GameObject> childrenWithTag;

    void Start()
    {
        if (searchTag != null)
        {
            FindAllChildren(transform);
            GetChildObjectsWithTag(searchTag);
        }
    }

    public void FindAllChildren(Transform transform)
    {
        int len = transform.childCount;

        for (int i = 0; i < len; i++)
        {
            allChildren.Add(transform.GetChild(i).gameObject);

            if (transform.GetChild(i).childCount > 0)
                FindAllChildren(transform.GetChild(i).transform);
        }
    }

    public void GetChildObjectsWithTag(string _tag)
    {
        foreach (GameObject child in allChildren)
        {
            if (child.tag == _tag)
                childrenWithTag.Add(child);
        }
    }
}