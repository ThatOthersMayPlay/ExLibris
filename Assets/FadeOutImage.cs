using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    //Show a message for a certain time:
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

    public float touchTime = 0.0f;

    public int mainScene = -1;
    public bool skipToMainScene = false;

    public bool useBuildIndex = false;
    public int buildInd = -1;

    // Start is called before the first frame update
    void Start()
    {
        fadeOutFactor = 255.0f / timer;
        fadeColor = fadeOutUI.color;
        fadeTimer = timer * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touchTime += Time.deltaTime;
            if (Input.touches[0].phase == TouchPhase.Ended && touchTime < 0.2f)
            {
                if (skipToMainScene && mainScene != -1)
                    SceneManager.LoadScene(mainScene);
                else
                    MenuControl.NextLevel(lv);
            }
        }
        else
            touchTime = 0.0f;

        //only 
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
            {
                if (useBuildIndex && buildInd != -1)
                    SceneManager.LoadScene(buildInd);
                else
                    MenuControl.NextLevel(lv);
            }
        }
    }
}
