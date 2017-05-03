using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;

public class AudioManager : MonoBehaviour {

    public bool OnFire;
    public int OnFireComboNumber = 2;
    [Space(20)]
    public AudioSource[] SoundAudioSources;
    public AudioSource[] MusicAudioSources;
    public AudioSource[] BassAudioSource;
    // Use this for initialization
    void Start () {
        ScoreController.ComboUpdateEvent += ScoreController_ComboUpdateEvent;
        ScoreController.ComboResetEvent += ScoreController_ComboResetEvent;
        GameController.StartEvent += delegate { PlayMusic(0); };
        GameController.StartEvent += delegate { PlayBass(0); };

        ScoreController.ComboResetEvent += delegate { PlaySound(0); };
        MatchController.OnMatchEventHappen += delegate { PlaySound(1); };
        RingFactory.onRefreshSetEvent += delegate { PlaySound(2); };
        GameController.CountDownOverEvent += delegate { PlaySound(3);  };
        GameController.RemoveColorTiersEvent += delegate { PlaySound(4); };
    }

    private void ScoreController_ComboResetEvent()
    {
        if (OnFire)
        {
            OnFire = false;
            PlayMusic(0);
            PlayBass(0);
        }
    }

    private void ScoreController_ComboUpdateEvent(int valueToReturn)
    {
        if (!OnFire && valueToReturn >= OnFireComboNumber)
        {
            OnFire = true;
            PlayMusic(1);
            PlayBass(1);
        }
    }

    private void PlaySound(int index)
    {
           SoundAudioSources[index].PlayOneShotSoundManaged(SoundAudioSources[index].clip);
    }
    private void PlayMusic(int index)
    {
        MusicAudioSources[index].PlayLoopingMusicManaged(1.0f, 1.0f, true);
    }
    private void PlayBass(int index)
    {
        BassAudioSource[index].PlayLoopingSoundManaged();
    }
}
