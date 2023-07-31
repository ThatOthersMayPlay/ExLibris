using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroControl : MonoBehaviour
{
    //Controls Intro UI behaviour


    //array of texts to appear after each other:
    public GameObject[] txtObjects;

    //array of progress bar objects to appear after each other:
    public Image[] progBarImgs;

    //array of duration in seconds till next text appears:
    public float[] txtDuration;

    //float that is counted down till zero to cause text to switch:
    public float counter = -1.0f;

    //index of texts:
    public int txtIndex = 0;

    //intro can be paused by touch and hold:
    public bool paused = false;

    //one time trigger for starting intro:
    public bool initialized = false;

    //HintManager Object:
    public HintManager hm;

    //Set Audio Source to play the reading of the texts
    public AudioSource audSrc;

    //Array of read intro texts:
    public AudioClip[] audClips;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   //intro can be paused by touch and hold:
        if (initialized) { PauseByTouch(); }
        CountDown(paused);

        if (!hm.HintsDisplayed())
            InitializeIntro(initialized);
    }

    public void CountDown(bool psd)
    {
        if (!psd)
        { 
            //count till zero:
            if (counter > 0.0f)
                counter -= Time.deltaTime;
            else
                NextHint();
        }
    }

    public void NextHint()
    {
        //Debug.Log("Counter is 0");
        //deactivate current text:
        SetTextActiveState(txtIndex, false);

        //if not last text then proceed:
        if (!IsLastText())
        {   //raise index:
            RaiseIndex();
            //Activate new progress bar and text object:
            SetTextActiveState(txtIndex, true);
            SetProgBarActiveState(txtIndex, true);
            counter = txtDuration[txtIndex];
            PlayAudio(txtIndex);
        }
        else
            CheckFirstStartThenLoadNextLv();
    }

    public void CheckFirstStartThenLoadNextLv()
    {
        if (HintManager.FirstStartDone())
            MenuControl.NextLevel(1);
        else
            MenuControl.NextLevel();
    }

    //Play a audio clip with the given index:
    public void PlayAudio(int ind)
    {
        audSrc.Stop();
        audSrc.clip = audClips[ind];
        audSrc.Play();
    }

    public void LoadHint(int ind)
    {
        paused = true;
        txtIndex = ind;

        for (int i = 0; i < txtObjects.Length; i++)
        {
            if (ind == i)
            {
                SetTextActiveState(i, true);
                counter = txtDuration[i];
            }
            else
                SetTextActiveState(i, false);

            if (ind >= i)
                SetProgBarActiveState(i, true);
            else
                SetProgBarActiveState(i, false);
        }
        paused = false;
    }

    public void SetTextActiveState(int ind, bool active)
    {   //set text with given index in/active:
        txtObjects[ind].SetActive(active);
    }

    public void SetProgBarActiveState(int ind, bool active)
    {   //set progress bar with given index in/active:
        if (active)
            progBarImgs[ind].color = Color.white;
        else
            progBarImgs[ind].color = new Color(0, 0, 0, 0);
    }
    
    //check if last index in text array:
    public bool IsLastText()
    {
        if (txtObjects.Length <= txtIndex + 1)
        {
            Debug.Log("Is last index");
            return true;
        }
        else
            return false;
    }

    public void RaiseIndex()
    {
        //raise text index:
        txtIndex++;
    }

    //intro can be paused by touch and hold:
    public void PauseByTouch()
    {
        //if (Input.touchSupported)
        //{
            if (Input.touchCount > 0)
                paused = true;
            else
                paused = false;
        //}
    }

    public void InitializeIntro(bool init)
    {
        if (!init)
        {
            paused = false;
            SetProgBarActiveState(0, true);
            PlayAudio(0);
            initialized = true;
        }
    }
}
