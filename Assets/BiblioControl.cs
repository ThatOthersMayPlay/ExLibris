using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BiblioControl : MonoBehaviour
{
    //Control of biblio level elements like animations, perspective etc.

    //reference audio source for finished scene:
    public AudioSource audSrc;

    //buttons to show up when first animation is finished:
    public GameObject[] charButtons;

    //in game menu buttons:
    public GameObject[] inGameButtons;
    public bool inGameButShown = false;

    //main menu buttons:
    public GameObject[] mainMenu;
    public bool mainMenuShown = false;

    public GameObject[] playedButtons;

    //menu buttons to minimize afer a certain time:
    public GameObject[] hideMenButtons;
    public GameObject[] showMenButtons;
    public float hideTime = 2.5f;
    //private bool hidden = false;

    //game speed for testing / adjustment:
    public float speed = 1.0f;
    public Animator[] anims;

    public bool started = false;
    public bool charButShown = false;



    //Special characters for better understanding in Start() and Update():
    public enum Chars
    {
        Menu = -1, Map = 0, BlindMan = 1, Jesus = 2,
        Des1 = 3, Des2 = 4, GetUp = 5, Des4 = 6, BeQuiet = 7,
        Cust1 = 8, Cust2 = 9, Cust3 = 10, Cust4 = 11, Cust5 = 12
    }
    public static Chars character = Chars.Map;

    public int tempChar = -1;

    //Availability and activation of Gyroscope control:
    public static bool useSensors = true;
    public static bool gyroAvailable = false;

    //buttons to show when gyposcope available or not:
    public GameObject sensOnBut;
    public GameObject sensOffBut;
    public GameObject sensNABut;
    //public GameObject testBut;

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

    //Buttons, Particle System and Time for buttons to appear (speaking out phrases):
    public GameObject[] phraseBut;
    //public ParticleSystem[] phraseFire;
    public float[] phraseStartTime;
    public float[] phraseEndTime;
    public float gameTimer = 0.0f;
    public float phraseButAlpha = 1.0f;
    public float phraseButHideTime = 1.0f;
    public float phraseButHideTimer = 0.0f;

    //black transparent image to darken sight of blind man:
    public GameObject blackImage;
    public float healingTimer = 0.0f;
    public float healingTime = 72.0f;

    //control play menu for play and pause and progressbar:
    public float pauseMenuTime = 1.0f;
    public float pauseMenuTimer = 1.0f;
    public GameObject pauseMenuByTabBut;
    public GameObject[] pauseMenButtons;
    public GameObject pauseBut;

    //manage progress bar (shown in pause menu):
    public Rect fullBar;
    public Rect progressBar;
    public RectTransform progressBarT;
    public float pixelsPerSec = 1.0f;

    ////sensor mode or follow mode:
    //public static bool sensorMode = true;

    //Selection of Animation to trigger at Start according to follow mode or sensor mode:
    public Animator[] charAnims;

    // Start is called before the first frame update
    void Start()
    {
        ConfigProgressBar();

        if (GetSceneTime() > 0.1f && character != Chars.Menu)
        {
            ApplySceneTime();
        }
        else
            SetSceneTime(0.0f);

        if (character == Chars.BlindMan)
        {
            blackImage.SetActive(true);
        }

            if (character == Chars.Menu)
        {
            //ShowCharButtons();
            SetSpeed(0.0f);
            HideUIElement(menuPauseButton);
            //ShowUIElement(menuPlayButton);
            HideUIElement(menuPlayButton);

            ShowMainMenu(true);

            LoadSensorMode();

            ActivateSensorButtons();

        //// Make sure device supprts Gyroscope
        //if (SystemInfo.supportsGyroscope)
        //{
        //    //Debug.Log("Device does support Gyroscopoe");
        //    gyroAvailable = true;
        //    sensNABut.SetActive(false);
        //        //testBut.SetActive(false);
        //}
        //else
        //    sensOnBut.SetActive(false);

            ShowInGameButtons(false);

            //if (!PlayerPrefs.HasKey("bibleHint"))
            //{
            //    HintManager hM = GameObject.Find("HintManager").GetComponent<HintManager>();
            //    hM.ShowHints(true);
            //    PlayerPrefs.SetInt("bibleHint",1);
            //}

            //SetCam(Chars.Map);

            //hide invisible button to activate pause menu:
            pauseMenuByTabBut.SetActive(false);

        } //Hold Animation till hint message have been closed:
        else
        {
            ShowMainMenu(false);
        }
        //if (initialPause)
        //    SetSpeed(0.0f);

        //Put Characters camera in front:
        if (character != Chars.Menu)
            cams[(int)character].depth = 5;

        //Activate Characters animation with or without animated camera according to follow mode or sensor mode:
        if (character != Chars.Menu)
        {
            if (gyroAvailable && useSensors)
                charAnims[(int)character].SetTrigger("TgBM");
            else
                charAnims[(int)character].SetTrigger("TgBMC");
        }

        ////Refresh played characters check symbol:
        //if (charButtons.Length == playedButtons.Length)
        //{   //beginning at index 1 (0 = map)
        //    for (int i = 1; i < charButtons.Length; i++)
        //    {
        //        if (PlayerPrefs.HasKey("CharPlayed" + i.ToString()))
        //            GameObject.Find("MenuControl").GetComponent<MenuControl>().ShowUIElement(playedButtons[i]);
        //    }
        //}
    }

    //public void FirePhrase()
    //{
    //    phraseFire[(int)character].Play();
    //}

    //Load Sensor mode from PlayerPrefs and deactivate sensor buttons when no sensor available:
    public void LoadSensorMode()
    {
        // Make sure device supprts Gyroscope
        if (!SystemInfo.supportsGyroscope)
        {
            gyroAvailable = false;
            //sensOnBut.SetActive(false);
            //sensOffBut.SetActive(false);
            //sensNABut.SetActive(true);
            SensorModeOn(false);
        }
        else if (PlayerPrefs.HasKey("sensorMode"))
        {
            if (PlayerPrefs.GetString("sensorMode") == "on")
            {
                //sensOnBut.SetActive(true);
                //sensOffBut.SetActive(false);
                //sensNABut.SetActive(false);
                SensorModeOn(true);
            }
            else if (PlayerPrefs.GetString("sensorMode") == "off")
            {
                //sensOnBut.SetActive(false);
                //sensOffBut.SetActive(true);
                //sensNABut.SetActive(false);
                SensorModeOn(false);
            }
        }
    }

    public void ActivateSensorButtons()
    {
        // Make sure device supprts Gyroscope
        if (!SystemInfo.supportsGyroscope)
        {
            sensOnBut.SetActive(false);
            sensOffBut.SetActive(false);
            sensNABut.SetActive(true);
        }
        else if (useSensors)
            {
                sensOnBut.SetActive(true);
                sensOffBut.SetActive(false);
                sensNABut.SetActive(false);
            }
            else
            {
                sensOnBut.SetActive(false);
                sensOffBut.SetActive(true);
                sensNABut.SetActive(false);
            }
    }

    //Save Sensor mode to PlayerPrefs:
    public void SaveSensorMode()
    {
        // Make sure device supprts Gyroscope
        if (SystemInfo.supportsGyroscope)
        {
            if (useSensors)
                PlayerPrefs.SetString("sensorMode", "on");
            else
                PlayerPrefs.SetString("sensorMode", "off");
        }
    }

    //switch between sensor and follow mode:
    public void SensorModeOn(bool on)
    {
        useSensors = on;
        //SaveSensorMode();
    }

    public void FireHideBut()
    {
        Image fireBut = phraseBut[(int)character].GetComponent<Image>();
        Color newCol = fireBut.color;
        newCol.a = 0.0f;
        fireBut.color = newCol;
    }

    //hide pause menu with timer:
    public void HidePauseMenu(bool hide)
    {
        if (hide)
        {
            if (!pauseMenuByTabBut.activeInHierarchy && pauseMenuTimer < pauseMenuTime)
            {
                pauseMenuTimer += Time.deltaTime;
            }
            else
            {
                pauseMenuByTabBut.SetActive(true);
                for (int i = 0; i < pauseMenButtons.Length; i++)
                    pauseMenButtons[i].SetActive(false);
            }
        }
        else if (pauseMenuTimer > 0.0f)
            pauseMenuTimer = 0.0f;
    }

    //hide pause menu without timer:
    public void HidePauseMenu()
    {
        pauseMenuByTabBut.SetActive(true);
        for (int i = 0; i < pauseMenButtons.Length; i++)
            pauseMenButtons[i].SetActive(false);
    }

    public void ConfigProgressBar()
    {
        progressBar = progressBarT.rect;
        float clipSeconds = audSrc.clip.length;
        pixelsPerSec = progressBar.width / clipSeconds;
    }

    public void UpdateProgressBar(bool upd)
    {
        
        progressBar.width = gameTimer * pixelsPerSec;
        Vector2 newRect = new Vector2(progressBar.width, progressBar.height);
        progressBarT.sizeDelta = newRect;
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
        //Activate phrase control / particle system of current character:
        if (speed == 1.0f)
            gameTimer += Time.deltaTime;

        //if other character than menu is selected:
        if ((int)character < phraseBut.Length && (int)character > -1)
        {
            //Debug.Log(((int)character).ToString());
            //activate phrase button at certain time:
            if (gameTimer > phraseStartTime[(int)character])
                phraseBut[(int)character].SetActive(true);
            //deactivate phrase button at certain time:
            if (gameTimer > phraseEndTime[(int)character])
                phraseBut[(int)character].SetActive(false);

            //make fire phrase button transparent for a certain time when fired:
            Image fireBut = phraseBut[(int)character].GetComponent<Image>();
            if (phraseButAlpha == 1.0f)
                phraseButAlpha = fireBut.color.a;

            if (fireBut.color.a == 0.0f && phraseButHideTimer < phraseButHideTime)
                phraseButHideTimer += Time.deltaTime;
            else
            {
                Color newCol = fireBut.color;
                newCol.a = phraseButAlpha;
                fireBut.color = newCol;
                phraseButHideTimer = 0.0f;
            }

            //deactivate pause and progressbar elements after a certain time when pause is not pressed:
            HidePauseMenu(pauseBut.activeInHierarchy);
            UpdateProgressBar(!pauseMenuByTabBut.activeInHierarchy);
        }

        //deactivate UI image that darkens the screen of the blind man, when he gets healed:
        if (character == Chars.BlindMan && blackImage.activeInHierarchy)
        {
            if (healingTimer < healingTime)
                healingTimer += Time.deltaTime;
            else
                blackImage.SetActive(false);
        }

        //Skip hint for continuing level (back from settings or bible text etc.):
        //if (!started)
        //{
        //    if (GetSceneTime() > 0.1f && character != Chars.Menu)
        //    {
        //        HintManager hM = GameObject.Find("HintManager").GetComponent<HintManager>();
        //        if (hM.HintsDisplayed()) { hM.ShowHints(false); }
        //    }
        //}

        //Set animaiton high speed to catch audio time, that has been set by stored time value:
        AnimHighSpeed(jumpAnimToAudio && !initialPause);

        //Start Animation after hint message have been closed (if menu isn't active):
        //StartAfterHint(!CharButtonsShown());

        //Hide Option buttons after some time to help fucus on game:
        //if (!hidden)
        //{ 
        //    if (!initialPause && hideTime > 0.0f) { hideTime -= Time.deltaTime; }
        //    HideObjWhenZero(hideMenButtons, hideTime);
        //    if (ShowObjWhenZero(showMenButtons, hideTime)) { hidden = true; }
        //}

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
        //Debug.Log("SetCamAndRestart(" + ch.ToString() + ")");
        //Debug.Log("SceneManager.LoadScene(" + (scene.buildIndex - 1).ToString() + ")");
        //SceneManager.LoadScene(scene.buildIndex - 1);
        //now reload current scene instead of "in between scene":
        SceneManager.LoadScene(scene.buildIndex);
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
        //if (speed == 1.0f)
        //    speed = 7.0f;
        //else
            speed = 1.0f;
    }
    public void Pause()
    {
        //if (speed == 1.0f)
        //    speed = 7.0f;
        //else
        speed = 0.0f;
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

    public void ShowInGameButtons(bool activate)
    {
        inGameButShown = activate;
        if (inGameButtons.Length > 0)
        {
            for (int i = 0; i < inGameButtons.Length; i++)
                inGameButtons[i].SetActive(activate);
        }
    }

    public void ShowMainMenu(bool activate)
    {
        mainMenuShown = activate;
        if (mainMenu.Length > 0)
        {
            for (int i = 0; i < mainMenu.Length; i++)
                mainMenu[i].SetActive(activate);
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
