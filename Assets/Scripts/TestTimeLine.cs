using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.Playables;
public class TestTimeLine : MonoBehaviour
{
    
    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 100, 100), "TimeLine"))
        {
            PlayTimeLine();
        }
    }
    void PlayTimeLine()
    {
        var timeLine = GetComponent<PlayableDirector>();
        timeLine.Play();
    }
  
}
