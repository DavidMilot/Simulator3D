/*
 * Author: David Milot
 * Create and control all audio sources via an object pool pattern.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioClip[] _clips;

    List<AudioSource> _audioSources = new List<AudioSource>();

    [SerializeField]
    private AudioSource _audioSource;

    public enum AudioSoundEffects
    {
        Click,
        PickUpSyringe,
        PickupVial,
        PickupGloveBox,
        PickupSanitizer,
        DropItem,
        HospitalAmbient
    }

    void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            _audioSources.Add(Instantiate(_audioSource));
            _audioSources[i].transform.SetParent(this.transform);
        }
    }

    public void PlaySound(AudioSoundEffects effect, bool loop, bool randomPitch)
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].isPlaying == false)
            {
                if (randomPitch)
                {
                    _audioSources[i].pitch = Random.Range(0.95f, 1.0f);
                }
                else
                {
                    _audioSources[i].pitch = 1.0f;
                }
                _audioSources[i].loop = loop;
                _audioSources[i].clip = _clips[(int)effect];
                _audioSources[i].Play();
                break;
            }
        }
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            _audioSources[i].Stop();
            _audioSources[i].loop = false;
        }
    }

    public void StopSound(AudioSoundEffects effects)
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].isPlaying == true && _audioSources[i].clip == _clips[(int)effects])
            {
                _audioSources[i].Stop();
                break;
            }
        }
    }
}
