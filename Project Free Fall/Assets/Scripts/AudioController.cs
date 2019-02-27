using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    protected AudioSource[] sounds;

    // Start is called before the first frame update
    public virtual void Start(){
        sounds = GetComponents<AudioSource>();
        
    }

    public void PlaySingleSound(string sound) {
        AudioSource audio = GetSource(sound);
        if (audio) {
            if (!audio.isPlaying) {
                audio.Play();
            }
        }
    }

    public void StopSingleSound(string sound) {
        AudioSource audio = GetSource(sound);
        if (audio) {
            if (audio.isPlaying) {
                audio.Stop();
            }
        }
    }

    protected AudioSource GetSource(string sound) {
        foreach(AudioSource audio in sounds) {
            if(audio.clip.name == sound) {
                return audio;
            }
        }

        return null;
    }

}
