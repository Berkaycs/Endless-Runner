using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource source;

    public AudioClip coin, clickButton, death;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayPickupCoin()
    {
        source.clip = coin;
        source.Play();
    }

    public void PlayClickButton()
    {
        source.clip = clickButton;
        source.Play();
    }

    public void PlayInDeath()
    {
        source.clip = death;
        source.Play();
    }
}
