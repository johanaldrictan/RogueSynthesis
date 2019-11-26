using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Dialogue : MonoBehaviour
{
    public LevelChanger levelChanger;
    public Animator animator;

    public Image portrait;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI textDisplay;

    EVENT_CALLBACK dialogueCallback;

    [FMODUnity.EventRef]
    public string dialogueEvent;

    EventInstance dialogueInstance;

    public Dialog[] dialogArray;

    private int index;
    public float typingSpeed = 0.02f;
    //private bool complete;

    void Start()
    {
        dialogueCallback = new EVENT_CALLBACK(DialogueEventCallback);
        portrait.sprite = dialogArray[index].portrait;
        nameDisplay.text = dialogArray[index].name;
        textDisplay.text = "";
        PlayDialogue(dialogArray[index].dialogueSoundKey);
        StartCoroutine(Type());
    }

    void Update()
    {
        if (textDisplay.text == dialogArray[index].sentence && Input.GetKeyDown("space"))
        {
            StopPreviousDialogue();
            NextSentence();
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in dialogArray[index].sentence)
        {
            textDisplay.text += letter;
            if (Input.GetKey(KeyCode.Space))
            {
                yield return new WaitForSeconds(0.001f);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
            
        }
    }

    public void NextSentence()
    {
        if (index < dialogArray.Length - 1)
        {
            index++;
            PlayDialogue(dialogArray[index].dialogueSoundKey);
            portrait.sprite = dialogArray[index].portrait;
            nameDisplay.text = dialogArray[index].name;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            Proceed();
        }
    }

    public void Proceed()
    {
        portrait.sprite = null;
        nameDisplay.text = "";
        textDisplay.text = "";
        //complete = true;
        //Debug.Log("Next");
        animator.SetTrigger("New Trigger");
        if (levelChanger != null)
        {
            levelChanger.GetComponent<LevelChanger>().FadeToNextLevel();
        }
    }

    void PlayDialogue(string key)
    {
        dialogueInstance = FMODUnity.RuntimeManager.CreateInstance(dialogueEvent);
        GCHandle stringHandle = GCHandle.Alloc(key, GCHandleType.Pinned);
        dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        dialogueInstance.setCallback(dialogueCallback);
        dialogueInstance.start();
        //dialogueInstance.release();
    }

    void StopPreviousDialogue()
    {
        dialogueInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT DialogueEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr)
    {
        IntPtr stringPtr;
        instance.getUserData(out stringPtr);

        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        string key = stringHandle.Target as string;

        switch (type)
        {
            case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
                {
                    FMOD.MODE soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING;
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

                    if (key.Contains("."))
                    {
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + key, soundMode, out dialogueSound);
                        if (soundResult == FMOD.RESULT.OK)
                        {
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = -1;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    else
                    {
                        FMOD.Studio.SOUND_INFO dialogueSoundInfo;
                        var keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);
                        if (keyResult != FMOD.RESULT.OK)
                        {
                            break;
                        }
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data, soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);
                        if (soundResult == FMOD.RESULT.OK)
                        {
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                }
                break;
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                {
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound();
                    sound.handle = parameter.sound;
                    sound.release();
                }
                break;
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                stringHandle.Free();
                break;
        }
        return FMOD.RESULT.OK;
    }

}
