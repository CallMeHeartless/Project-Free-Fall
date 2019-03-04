using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    private AudioSource music;

    private void Awake() {
        GameObject[] instances = GameObject.FindGameObjectsWithTag("Music");
        if(instances.Length > 1) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartMusic() {
        if(music == null) {
            music = GetComponent<AudioSource>();
        }

        if (music.isPlaying) {
            return;
        }
        music.Play();
    }

    public void Stop() {
        if(music != null) {
            music.Stop();
        }
    }

}
