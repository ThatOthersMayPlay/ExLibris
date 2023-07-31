using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InBetweenControl : MonoBehaviour
{   //Controlling Animation of hints "reflect?" / "notes?" / "pray?" fading in and out one after another

    //text elements to fade in and out:
    public Text[] hintTexts;

    //textures to fade in and out:
    public Image[] hintImgaes;

    public float fadeTimer = 1.0f;

    public float fadeSpeed = 1.0f;

    public int hintIndex = 0;

    public bool freezeHint = false;

    public bool fadeOut = true;

    //keep one type of hint to be faded in and out and don't switch to the other two
    public void FreezeHint()
    {
        freezeHint = !freezeHint;

        if (freezeHint)
            hintTexts[hintIndex].text = hintTexts[hintIndex].text.Substring(0, hintTexts[hintIndex].text.Length - 1);
        else
            hintTexts[hintIndex].text = hintTexts[hintIndex].text + "?";
    }


    // Update is called once per frame
    void Update()
    {
        if (freezeHint)
        {
            Color hC = hintTexts[hintIndex].color;
            float hCa = hC.a;
            hCa = 1.0f;
            hintTexts[hintIndex].color = new Color(hC.r, hC.g, hC.b, hCa);
            hintImgaes[hintIndex].color = new Color(hC.r, hC.g, hC.b, hCa);
            fadeOut = true;
            fadeTimer = 1.0f;
        }
        else
        {
            if (fadeOut)
            {
                if (fadeTimer > 0.0f)
                {
                    fadeTimer -= Time.deltaTime * fadeSpeed;
                    if (fadeTimer > 0.0f)
                    {

                        Color hC = hintTexts[hintIndex].color;
                        float hCa = hC.a;
                        if (hC.a > 0.0f)
                        {
                            if (hC.a - Time.deltaTime * fadeSpeed > 0.0f)
                                hCa = hCa - Time.deltaTime * fadeSpeed;
                            else
                                hCa = 0.0f;
                        }
                        hintTexts[hintIndex].color = new Color(hC.r, hC.g, hC.b, hCa);
                        hintImgaes[hintIndex].color = new Color(hC.r, hC.g, hC.b, hCa);
                    }
                    else
                    {
                        fadeOut = false;
                        fadeTimer = 1.0f;
                        if (!freezeHint)
                        {
                            if (hintIndex != 2)
                                hintIndex++;
                            else
                                hintIndex = 0;
                        }
                    }
                }
            }
            else
            {
                if (fadeTimer > 0.0f)
                {
                    fadeTimer -= Time.deltaTime * fadeSpeed;
                    if (fadeTimer > 0.0f)
                    {

                        Color hC = hintTexts[hintIndex].color;
                        float hCa = hC.a;
                        if (hC.a < 1.0f)
                        {
                            if (hC.a + Time.deltaTime * fadeSpeed < 1.0f)
                                hCa = hCa + Time.deltaTime * fadeSpeed;
                            else
                                hCa = 1.0f;
                        }
                        hintTexts[hintIndex].color = new Color(hC.r, hC.g, hC.b, hCa);
                        hintImgaes[hintIndex].color = new Color(hC.r, hC.g, hC.b, hCa);
                    }
                    else
                    {
                        fadeOut = true;
                        fadeTimer = 1.0f;
                    }
                }
            }
        }
    }
}
