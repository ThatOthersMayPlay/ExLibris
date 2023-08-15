using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RotateCam : MonoBehaviour
{
    public Gyroscope gyros;
    //public bool gyroAvailable = true;
    public float xx = 0.0f;
    //public float yy = 0.0f;
    //public float zz = 0.0f;
    public int[] a = new int[48];
    public int[] b = new int[48];
    public int[] c = new int[48];

    public int testI = 1;

    public float rotatespeed = 1.0f;

    //touch control:
    public Vector2 tStartPos;

    // Start is called before the first frame update
    void Start()
    {
        if (BiblioControl.gyroAvailable && BiblioControl.useSensors)
        {
            gyros = Input.gyro;
            gyros.enabled = true;    // Must enable the gyroscope
            FollowMobile(BiblioControl.useSensors);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BiblioControl.useSensors && BiblioControl.gyroAvailable)
            FollowMobile(true);

        //RotateByTouch(BiblioControl.useSensors);
    }

    void FollowMobile(bool follow)
    {
        if (follow)
            transform.rotation = Quaternion.Lerp(transform.rotation, gyros.attitude, Time.deltaTime * rotatespeed);
    }

    //void RotateByTouch(bool byTouch)
    //{
    //    if (!byTouch)
    //    {
    //        if (Input.touchCount == 1)
    //        {
    //            Touch myT = Input.GetTouch(0);
    //            if (myT.phase == TouchPhase.Began)
    //            {
    //                tStartPos = myT.position;
    //            }
    //            else if (myT.phase == TouchPhase.Moved)
    //            {

    //                //float mySp = Time.deltaTime * rotatespeed;
    //                transform.Rotate(myT.deltaPosition.y * Time.deltaTime * 40, -myT.deltaPosition.x * Time.deltaTime * 40, 0, Space.Self);
    //                //transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation, Time.deltaTime * rotatespeed);
    //            }
    //        }
    //    }
    //}

    public float GetTestV(int inp)
    {
        float retFl = 0.0f;
        if (inp == 1)
            retFl = gyros.attitude.eulerAngles.x;
        if (inp == -1)
            retFl = -gyros.attitude.eulerAngles.x;
        if (inp == 2)
            retFl = gyros.attitude.eulerAngles.y;
        if (inp == -2)
            retFl = -gyros.attitude.eulerAngles.y;
        if (inp == 3)
            retFl = gyros.attitude.eulerAngles.z;
        if (inp == -3)
            retFl = -gyros.attitude.eulerAngles.z;

        return retFl;
    }
}
