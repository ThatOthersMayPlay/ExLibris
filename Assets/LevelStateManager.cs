using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStateManager : MonoBehaviour
{
    //Script to control level variables according to game state like recent level or paused game

    public Button nextLvBut;
    public Text nextLvButText;

    // Start is called before the first frame update
    void Start()
    {
        if (BiblioControl.sceneTime > 0.0f)
        {
            nextLvButText.text = "back";
            nextLvBut.onClick.RemoveAllListeners();
            nextLvBut.onClick.AddListener(() => MenuControl.NextLevel(2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
