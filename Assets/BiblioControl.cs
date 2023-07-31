using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BiblioControl : MonoBehaviour
{
    //Control of biblio level elements like animations, perspective etc.

    //reference audio source for finished scene:
    public AudioSource audSrc;

    //buttons to show up when first animation is finished:
    public GameObject[] charButtons;

    public GameObject[] playedButtons;

    //menu buttons to minimize afer a certain time:
    public GameObject[] hideMenButtons;
    public GameObject[] showMenButtons;
    public float hideTime = 2.5f;
    private bool hidden = false;

    //game speed for testing / adjustment:
    public float speed = 1.0f;
    public Animator[] anims;

    public bool started = false;
    public bool charButShown = false;

    
    //Special characters for better understanding in Start() and Update():
    public enum Chars
    {
        Menu = -1, Map = 0, Barto = 1, Jesus = 2,
        Des1 = 3, Des2 = 4, Des3 = 5, Des4 = 6, Des5 = 7,
        Cust1 = 8, Cust2 = 9, Cust3 = 10, Cust4 = 11, Cust5 = 12
    }
    public static Chars character = Chars.Map;

    public int tempChar = -1;

    public static bool useSenesors = true;

    public Camera[] cams;

    //speed control for debugging / adjusting animation:
    public bool speedControl = true;

    //set animation high speed to catch the current time of audio source:
    public bool jumpAnimToAudio = false;
    public float jumpAnimTimer = 2.0f;

    public static float sceneTime = 0.0f;

    public GameObject menuPlayButton;
    public GameObject menuPauseButton;

    public bool initialPause = true;

    // Start is called before the first frame update
    void Start()
    {
        if (GetSceneTime() > 0.1f && character != Chars.Menu)
        {
            ApplySceneTime();
        }
        else
            SetSceneTime(0.0f);


        if (character == Chars.Menu)
        {
            ShowCharButtons();
            //SetSpeed(0.0f);
            HideUIElement(menuPauseButton);
            ShowUIElement(menuPlayButton);

            if (!PlayerPrefs.HasKey("bibleHint"))
            {
                HintManager hM = GameObject.Find("HintManager").GetComponent<HintManager>();
                hM.ShowHints(true);
                PlayerPrefs.SetInt("bibleHint",1);
            }

            //SetCam(Chars.Map);
        } //Hold Animation till hint message have been closed:

        if (initialPause)
            SetSpeed(0.0f);

        //Put Characters camera in front:
        if (character != Chars.Menu)
            cams[(int)character].depth = 5;

        //Refresh played characters check symbol:
        if (charButtons.Length == playedButtons.Length)
        {   //beginning at index 1 (0 = map)
            for (int i = 1; i < charButtons.Length; i++)
            {
                if (PlayerPrefs.HasKey("CharPlayed" + i.ToString()))
                    GameObject.Find("MenuControl").GetComponent<MenuControl>().ShowUIElement(playedButtons[i]);
            }
        }
    }

    
    //Apply the stored point on the timeline to the animations and audio source:
    public void ApplySceneTime()
    {
        audSrc.time = sceneTime;
        jumpAnimToAudio = true;
    }

    //Store a point in the timeline of the audio source of a custom point (overload):
    public void SetSceneTime(float stime) { sceneTime = stime; }
    public void SetSceneTime() { sceneTime = audSrc.time; }

    //Get the stored point in the timeline (for animation and audio):
    public float GetSceneTime() { return sceneTime; }

    // Update is called once per frame
    void Update()
    {
        //Skip hint for continuing level (back from settings or bible text etc.):
        if (!started)
        {
            if (GetSceneTime() > 0.1f && character != Chars.Menu)
            {
                HintManager hM = GameObject.Find("HintManager").GetComponent<HintManager>();
                if (hM.HintsDisplayed()) { hM.ShowHints(false); }
            }
        }

        //Set animaiton high speed to catch audio time, that has been set by stored time value:
        AnimHighSpeed(jumpAnimToAudio && !initialPause);

        //Start Animation after hint message have been closed (if menu isn't active):
        StartAfterHint(!CharButtonsShown());

        //Hide Option buttons after some time to help fucus on game:
        if (!hidden)
        { 
            if (!initialPause && hideTime > 0.0f) { hideTime -= Time.deltaTime; }
            HideObjWhenZero(hideMenButtons, hideTime);
            if (ShowObjWhenZero(showMenButtons, hideTime)) { hidden = true; }
        }

        if (audSrc.isPlaying)
            started = true;

        if (!audSrc.isPlaying && started && !charButShown)
        {
            SetCamAndRestart((int)Chars.Menu);
        }

        if (speedControl)
        {
            if (!jumpAnimToAudio)
            {
                for (int i = 0; i < anims.Length; i++)
                    anims[i].speed = speed;
            }

            audSrc.pitch = speed;
        }
    }

    //Set animaiton high speed to catch audio time, that has been set by stored time value:
    public void AnimHighSpeed(bool jmp)
    {
        if (jmp)
        {
            float hSpeed = 0.5f * ((GetSceneTime()) + (1.0f / GetSceneTime()));

            if (jumpAnimTimer > 0.0f)
                jumpAnimTimer -= Time.deltaTime;
            else
            {
                jumpAnimTimer = 2.0f;
                hSpeed = 1.0f;
                jumpAnimToAudio = false;
            }

            for (int i = 0; i < anims.Length; i++)
            {
                //if (i == 1) { Debug.Log("AnimHighSpeed: "+anims[0].speed.ToString()); }
                anims[i].speed = hSpeed;
            }
        }
    }

    public void SetCamAndRestart(int ch)
    {
        SetCam((Chars)ch);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex-1);
    }

    public void SetCam(Chars ch)
    {
        character = ch;
    }

    public void SetCam(int ch)
    {
        character = (Chars)ch;
    }

    public void SetCamAndRestartPre()
    {
        if (tempChar != -1)
        {
            SetCam((Chars)tempChar);
            PlayerPrefs.SetInt("CharPlayed"+tempChar.ToString(), 1);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }

    public void SetTempChar(int ch)
    {
        tempChar = ch;
    }

    public void SetSpeed(float spd)
    {
            speed = spd;
    }

    public void Play()
    {
        if (speed == 1.0f)
            speed = 7.0f;
        else
            speed = 1.0f;
    }

    public void ActivateButAndRestart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ShowCharButtons()
    {
        charButShown = true;
        if (charButtons.Length > 0)
        {
            for (int i = 0; i < charButtons.Length; i++)
                charButtons[i].SetActive(true);
        }
    }

    public bool CharButtonsShown()
    {
        charButShown = false;
        if (charButtons.Length > 0)
        {
            for (int i = 0; i < charButtons.Length; i++)
                charButShown = charButtons[i].activeInHierarchy;

            if (!charButShown) { return false; }
        }
        return charButShown;
    }

    public void HideUIElement(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ShowUIElement(GameObject obj)
    {
        obj.SetActive(true);
    }

    //Start Animation after hint message have been closed:
    public void StartAfterHint(bool start)
    {
        if (start)
        {
            HintManager hM = GameObject.Find("HintManager").GetComponent<HintManager>();

            if (initialPause && !hM.HintsDisplayed())
            {
                SetSpeed(1.0f);
                initialPause = false;
            }
        }
    }

    public void HideObjWhenZero(GameObject[] hideObj, float timer)
    {
        if (hideObj[0].activeInHierarchy)
        {
            if (!(timer > 0.0f))
            {
                for (int i = 0; i < hideObj.Length; i++)
                {
                    GameObject.Find("MenuControl").GetComponent<MenuControl>().HideUIElement(hideObj[i]);
                }
            }
        }
    }

    public bool ShowObjWhenZero(GameObject[] showObj, float timer)
    {
        bool ret = false;
        if (!showObj[0].activeInHierarchy)
        {
            if (!(timer > 0.0f))
            {
                for (int i = 0; i < showObj.Length; i++)
                {
                    GameObject.Find("MenuControl").GetComponent<MenuControl>().ShowUIElement(showObj[i]);
                }

                ret = true;
            }
        }
        return ret;
    }
}
