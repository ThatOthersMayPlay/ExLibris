using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationControl : MonoBehaviour
{
    //Switch navigation mode

    public void SwitchSensorControl()
    {
        if (BiblioControl.useSensors)
            BiblioControl.useSensors = false;
        else
            BiblioControl.useSensors = true;
    }
}
