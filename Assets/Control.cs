using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Control : MonoBehaviour
{
    public VideoPlayer vPl;
    public VideoClip vCl1;
    public VideoClip vCl2;

    public AudioSource audioSrc;

    public Button rockBt;
    public Button playBt;
    public Button pauseBt;

    public Text holdText;

    public GameObject dummy;

    public bool released = false;

    public void Start()
    {
        //rockBt.gameObject.SetActive(false);
        //pauseBt.gameObject.SetActive(false);
        //playBt.enabled = false;
    }

    public void PlayVideo()
    {
        if (dummy.activeInHierarchy)
            dummy.SetActive(false);

        vPl.Play();
        audioSrc.Play();


        if (!released)
            rockBt.gameObject.SetActive(true);
        else
            pauseBt.gameObject.SetActive(true);

        playBt.gameObject.SetActive(false);

        if (holdText.gameObject.activeInHierarchy)
            holdText.gameObject.SetActive(false);
    }

    public void PauseVideo()
    {
        vPl.Pause();
        audioSrc.Pause();
        pauseBt.gameObject.SetActive(false);
        playBt.gameObject.SetActive(true);
    }


}
