using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundTimed : MonoBehaviour
{
    private float timer = 2.0f;
    private bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f) { timer -= Time.deltaTime; }

        if (!played)
        {
            if (!(timer > 0.0f))
            {
                GetComponent<AudioSource>().Play();
                played = true;
            }
        }
    }
}
