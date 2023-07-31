using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    //Advice UI Elements displayed at first start, disappears by simple touch
    public bool usePPfirstStart = true;
    public bool hintsStart = true;

    private bool touched = false;
    //private bool adviceOff = false;
    public GameObject[] adviceUIs;
    public MenuControl mC;

    public bool debugMode = true;

    private float timer = 0.0f;

    public static bool hintsOn = true;

    public bool manageHints = true;

    // Start is called before the first frame update
    void Start()
    {
        if (HintManager.FirstStartDone() && !debugMode && usePPfirstStart)
            touched = true;

        if (manageHints)
        {
            ShowHintIconOnOff(GetHintsOn());
        }

        if (hintsStart)
        {
            ShowHints(GetHintsOn());
        }

        //PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (!touched)
        {
            if (CooledDown())
            {
                if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
                    touched = true;
            }
        }
        else// if (!adviceOff)
        {
            ShowHints(false);

        }

            //Count down timer:
            if (timer > 0.0f) { timer -= Time.deltaTime; }
    }

    public void ShowHints(bool show)
    {
        if (show)
        {
            usePPfirstStart = false;
            timer = 0.5f;
            for (int i = 0; i < adviceUIs.Length; i++)
                mC.ShowUIElement(adviceUIs[i]);
        }
        else
        {
            for (int i = 0; i < adviceUIs.Length; i++)
                mC.HideUIElement(adviceUIs[i]);
            touched = false;
        }
    }

    //Check if hints are displayed at the moment:
    public bool HintsDisplayed() { return adviceUIs[0].activeInHierarchy; }

    //Activate the matching icon on the top right to show if hints are on or off:
    public void ShowHintIconOnOff(bool on)
    {
        GameObject onO = GameObject.Find("Canvas").transform.Find("HintsOn").gameObject;
        GameObject offO = GameObject.Find("Canvas").transform.Find("HintsOff").gameObject;
        if (on)
        {
            mC.ShowUIElement(onO);
            mC.HideUIElement(offO);
        }
        else
        {
            mC.ShowUIElement(offO);
            mC.HideUIElement(onO);
        }
    }


    //Cooldown timer running?:
    public bool CooledDown() { return !(timer > 0.0f); }

    //Check if first start of the app is logged in PlayerPrefs:
    public static bool FirstStartDone()    { return PlayerPrefs.HasKey("firstStart"); }
    //Only if first sart of the app should be detected:
    public void TriggerFirstStart() { if (usePPfirstStart) { PlayerPrefs.SetInt("firstStart", 1); } }
    
    //Check if hints on/off is stored in PlayerPrefs:
    public static bool HintsStored()    { return PlayerPrefs.HasKey("hintsOn"); }
    //Get hints on/off as stored in PlayerPrefs:
    public static bool GetHintsOn()    { if (PlayerPrefs.GetInt("hintsOn") == 0) { return false; } else { return true; } }

    //Set Hints On/Off in game and PlayerPrefs:
    public void SetHintsOn(bool on)
    {
        hintsOn = on;
        PlayerPrefs.SetInt("hintsOn", on ? 1:0);
        ShowHintIconOnOff(on);
    }
}