using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOutText : MonoBehaviour
{
    //Show a message for a certain time:
    public float timer = 0.0f;
    public float hideTime = 3.0f;
    public float fadeTimer = 1.0f;

    public Text fadeOutUI;

    public Color UIColor;
    public Color fadeColor;

    public float fadeOutFactor = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        fadeOutFactor = 255.0f / fadeTimer;
        fadeOutUI = GetComponent<Text>();
        UIColor = fadeOutUI.color;
        fadeColor = fadeOutUI.color;
        timer = hideTime;
        //fadeTimer = hideTime * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (gameObject.activeInHierarchy)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                if (timer <= fadeTimer)
                {
                    fadeColor.a -= Time.deltaTime;
                    fadeOutUI.color = fadeColor;
                }
            }//must be ready for next level:
            else
            {
                gameObject.SetActive(false);
                timer = hideTime;
                //fadeTimer = hideTime * 0.5f;
                fadeColor = UIColor;
                fadeOutUI.color = UIColor;
            }
        }
    }
}
