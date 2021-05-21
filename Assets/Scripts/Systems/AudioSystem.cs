using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    #region Core

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void PlayOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    #endregion

    [Header("UI Sounds")]
    [SerializeField] private AudioClip clickSound = null;
    [SerializeField] private AudioClip successSound = null;


    private void Start()
    {
        EventManager.StartListening(EventManager.clickSoundEvent, () => PlayOneShot(clickSound));
        EventManager.StartListening(EventManager.successSoundEvent, () => PlayOneShot(successSound));
    }
}
