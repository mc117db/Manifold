using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;

public class AudioManager : MonoBehaviour {

    public AudioSource[] SoundAudioSources;
    public AudioSource[] MusicAudioSources;
    // Use this for initialization
    void Start () {
        GameController.StartEvent += delegate { PlayMusic(0); };
        ScoreController.ComboResetEvent += delegate { PlaySound(0); };
        MatchController.OnMatchEventHappen += delegate { PlaySound(1); };
        RingFactory.onRefreshSetEvent += delegate { PlaySound(2); };
        GameController.CountDownOverEvent += delegate { PlaySound(3);  };
        GameController.RemoveColorTiersEvent += delegate { PlaySound(4); };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void PlaySound(int index)
    {
           SoundAudioSources[index].PlayOneShotSoundManaged(SoundAudioSources[index].clip);
    }
    private void PlayMusic(int index)
    {
        MusicAudioSources[index].PlayLoopingMusicManaged(1.0f, 1.0f, true);
    }
}
