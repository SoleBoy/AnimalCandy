using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get => instance;}
    public List<AudioClip> audios = new List<AudioClip>();
    public bool isPause;
    public float soundValue;
    public AudioSource bgSource;//背景音效
    private AudioSource touchTone;

    private Dictionary<string, AudioClip> pairs = new Dictionary<string, AudioClip>();
    public void Init()
    {
        instance = this;
        bgSource = GetComponent<AudioSource>();
        touchTone = transform.Find("node").GetComponent<AudioSource>();
        for (int i = 0; i < audios.Count; i++)
        {
            pairs.Add(audios[i].name, audios[i]);
        }
        bgSource.volume = PlayerPrefs.GetFloat("SliderMusic", 1);
        soundValue = PlayerPrefs.GetFloat("SliderSound", 1);
        //if (PlayerPrefs.GetString("Music") != "")
        //{
        //    bgSource.Pause();
        //}
        //if (PlayerPrefs.GetString("Sound") != "")
        //{
        //    isPause = true;
        //}
    }
    //切换背景
    public void CutBgMusic(string name)
    {
        bgSource.clip = pairs[name];
        bgSource.Play();
    }

    //播放特殊音效
    public void PlaySource(string name,AudioSource source,float value = 1)
    {
        if(source.gameObject.activeInHierarchy && soundValue > 0)
        {
            source.volume = soundValue * value;
            source.clip = pairs[name];
            source.Play();
        }
    }
    //按键声音
    public void PlayTouch(string name)
    {
        if (soundValue > 0)
        {
            touchTone.volume = soundValue;
            touchTone.clip = pairs[name];
            touchTone.Play();
        } 
    }
    public void RestoreSound(float value)
    {
        soundValue = value;
        isPause = soundValue <= 0;
    }
    //暂停
    public void PauseSource()
    {
        bgSource.Pause();
        isPause = true;
    }
    //播放
    public void RestoreSource()
    {
        bgSource.Play();
    }
}
