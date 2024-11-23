using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneKeep : MonoBehaviour
{
    private static List<string> ids = new();
    public string id = "";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (ids.Contains(id))
            Destroy(gameObject);
        else ids.Add(id);
    }
}
