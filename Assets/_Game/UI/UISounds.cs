using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class UISounds : MonoBehaviour
{
    EventInstance UIHover;
    EventInstance UIConfirm;
    EventInstance UICancel;
    // Start is called before the first frame update
    void Start()
    {
        UIHover = FMODUnity.RuntimeManager.CreateInstance("event:/UI/Select");
        UIConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/Confirm");
        UICancel = FMODUnity.RuntimeManager.CreateInstance("event:/UI/Cancel");
    }
    
    public void PlayConfirm()
    {
        UIConfirm.start();
    }
    public void PlayCancel()
    {
        UICancel.start();

    }

    private void OnHover()
    {
        //UIHover.start();
        Debug.Log("i am triggered");
    }
}
