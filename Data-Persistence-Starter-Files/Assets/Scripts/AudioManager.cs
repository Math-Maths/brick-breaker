using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager: MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource ambient;
    [SerializeField] private AudioSource fxs;

    [SerializeField] private AudioClip menuMusic, mainMusic, endMusic;
    [SerializeField] private AudioClip bounceFX, breakFX, loseFX, winFX;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code
    
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AmbientMusic(SceneManager.GetActiveScene().name);
    }

    public void AmbientMusic(string value)
    {
        ambient.volume = .5f;
        switch(value)
        {
            case "menu":
                ambient.clip = menuMusic;
                ambient.Play();
                break;
            case "main":
                ambient.clip = mainMusic;
                ambient.Play();
                break;
            case "end":
                ambient.clip = endMusic;
                ambient.volume = .3f;
                ambient.Play();
                break;
        }
    }

    public void PlayClip(string value)
    {
        fxs.volume = .6f;
        switch(value)
        {
            case "win":
                fxs.volume = 1;
                fxs.PlayOneShot(winFX);
                break;
            case "lose":
                fxs.PlayOneShot(loseFX);
                break;
            case "break":
                fxs.PlayOneShot(breakFX);
                break;
            case "bounce":
                fxs.PlayOneShot(bounceFX);
                break;
        }
    }

}