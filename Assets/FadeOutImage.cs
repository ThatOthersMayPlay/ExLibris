using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    //Show quotation note for NIV translation for a second
    public float timer = 3.0f;
    private float fadeTimer = 1.0f;

    public Image fadeOutUI;

    private Color fadeColor;

    private float fadeOutFactor = 1.0f;

    public bool toNextLevel = true;
    public int lv = 1;

    //enable wait for next level by touch:
    public bool waitForNoTouch = true;
    public bool readyForNextLv = false;

    // Start is called before the first frame update
    void Start()
    {
        fadeOutFactor = 255.0f / timer;
        fadeColor = fadeOutUI.color;
        fadeTimer = timer * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {   //only 
        if (waitForNoTouch)
        {   //check if touch active - then wait with level change:
            if (Input.touchCount > 0)
                readyForNextLv = false;
            else
                readyForNextLv = true;
        }
        else//if touch doesn't affect level change set ready for level change:
            readyForNextLv = true;

        if (readyForNextLv)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                if (timer <= fadeTimer)
                {
                    fadeColor.a += Time.deltaTime;
                    fadeOutUI.color = fadeColor;
                }
            }//must be ready for next level:
            else if (toNextLevel)
                MenuControl.NextLevel(lv);
        }
    }
}
