using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveMobileImage : MonoBehaviour
{
    //Animates a moving mobile phone using a series of images:
    public Sprite[] mobTex;
    public int mobInd = 0;

    public float animTime = 0.0f;
    public float animTimer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animTime < animTimer)
            animTime += Time.deltaTime;
        else
        {
            if (mobTex.Length > mobInd + 1)
                mobInd += 1;
            else
                mobInd = 0;

            Image mobIm = GetComponent<Image>();
            mobIm.sprite = mobTex[mobInd];

            animTime = 0.0f;
        }
    }
}
