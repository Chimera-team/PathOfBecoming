using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource effectsSound;
    [SerializeField] AudioSource musicSound;
    [SerializeField] AudioSource ambientSound;
    
    static public Queue<AudioClip> effectsQueue = new Queue<AudioClip>();
    static public Queue<AudioClip> musicQueue = new Queue<AudioClip>();
    static public Queue<AudioClip> ambientQueue = new Queue<AudioClip>();

    void Update()
    {
        Check_Effects_Queue();
        Check_Music_Queue();
        Check_Ambient_Queue();
    }

    void Check_Effects_Queue()
    {
        while (effectsQueue.Count > 0)
        {
            effectsSound.PlayOneShot(effectsQueue.Dequeue(), Engine.current.gameSettings.soundSettings.effectsVolume * Engine.current.gameSettings.soundSettings.masterVolume);
        }
    }

    void Check_Music_Queue()
    {
        if (musicQueue.Count > 0)
        {
            musicSound.clip = musicQueue.Dequeue();
            musicSound.Play();
        }
        if (Engine.current != null && musicSound.volume != Engine.current.gameSettings.soundSettings.musicVolume)
            musicSound.volume = Engine.current.gameSettings.soundSettings.musicVolume * Engine.current.gameSettings.soundSettings.masterVolume;
    }

    void Check_Ambient_Queue()
    {
        if (ambientQueue.Count > 0)
        {
            ambientSound.clip = ambientQueue.Dequeue();
            ambientSound.Play();
        }
        if ( Engine.current != null&&ambientSound.volume != Engine.current.gameSettings.soundSettings.effectsVolume )
            ambientSound.volume = Engine.current.gameSettings.soundSettings.effectsVolume * Engine.current.gameSettings.soundSettings.masterVolume;
    }

    public void Play_Effect(AudioClip clip)
    {
        effectsQueue.Enqueue(clip);
    }

    public void Play_Music(AudioClip clip)
    {
        musicQueue.Enqueue(clip);
    }

    public void Play_Ambient(AudioClip clip)
    {
        ambientQueue.Enqueue(clip);
    }
}