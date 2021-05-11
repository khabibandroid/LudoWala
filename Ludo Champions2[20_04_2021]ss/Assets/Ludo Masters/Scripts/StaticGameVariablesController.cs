﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticGameVariablesController : MonoBehaviour
{

    public static StaticGameVariablesController instance;
    public Sprite[] avatars;

    public Sprite[] urls;

    public Sprite[] emoji;
    
    public int emojiPerPack = 12;
    public int packsCount = 6;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }
}
