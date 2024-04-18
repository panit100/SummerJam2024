using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using CuteEngine.Utilities;

public class SoundManager : PersistentSingleton<SoundManager>
{

    public const string GAMEPLAY_BGM = "bgm-gameplay";
    public const string MENU_BGM = "bgm-menu";

    public AudioSource mainAudio;
    public float sfxAudioVolume = 0.5f;
    public SoundDatas soundDatas;
    public GameObject sfxPrefab;
    public int sfxCount = 10;
    public List<AudioSource> sfxAudios;

    protected override void InitAfterAwake()
    {
        var childobj = new GameObject();
        childobj.transform.SetParent(this.transform);
        var main = this.gameObject.AddComponent<AudioSource>();
        main.volume = 0.5f;
        var sfx = childobj.AddComponent<AudioSource>();
        sfxPrefab = Resources.Load<GameObject>("sfxAudio");
        sfxAudioVolume = 0.5f;
        sfxAudios = new List<AudioSource>();
        init();
        mainAudio = main;
        // data.sfxAudio = sfx;
        soundDatas = Resources.Load<SoundDatas>("SoundData");

        // mainAudio.volume = PlayerPrefs.GetFloat("bgmVolume");
        // sfxAudioVolume = PlayerPrefs.GetFloat("sfxVolume");
    }

    void Start()
    {

    }

    public void ChangeBGM(string fileName)
    {
        AudioClip c = soundDatas.allBGM.Find(clip => clip.name == fileName);
        mainAudio.clip = c;
        mainAudio.Play();
    }

    public void PlaySFX(string fileName, bool isLoop = default(bool))
    {
        AudioClip c = soundDatas.allSFX.Find(clip => clip.name == fileName);
        AudioSource audioSource = sfxAudios.Find(audio => audio.isPlaying == false);
        if (!audioSource)
        {
            GameObject obj = Instantiate(sfxPrefab);
            AudioSource auido = obj.GetComponent<AudioSource>();
            obj.transform.SetParent(this.gameObject.transform);
        }

        audioSource.volume = sfxAudioVolume;
        audioSource.loop = isLoop;
        audioSource.clip = c;
        audioSource.Play();
    }

    public void StopSFXLoop(string fileName)
    {
        AudioSource audioSource = sfxAudios.Find(audio => audio.clip.name == fileName && audio.loop == true);
        if (audioSource)
        {
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.Stop();
        }

    }

    public void init()
    {
        for (int x = 0; x < sfxCount; x++)
        {
            GameObject obj = Instantiate(sfxPrefab);
            AudioSource auido = obj.GetComponent<AudioSource>();
            auido.volume = sfxAudioVolume;
            obj.transform.SetParent(this.gameObject.transform);
            sfxAudios.Add(auido);
        }
        SetAllButton();
    }

    public void SetAllButton()
    {
        var allObj = Resources.FindObjectsOfTypeAll(typeof(Button));

        foreach (Button obj in allObj)
        {
            obj.GetComponent<Button>().onClick.AddListener(delegate { PlaySFX("button_cute_04"); });
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("bgmVolume", mainAudio.volume);
        PlayerPrefs.SetFloat("sfxVolume", sfxAudioVolume);
    }
}
