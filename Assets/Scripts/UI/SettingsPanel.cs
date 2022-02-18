using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Sprite soundNot;
    public Sprite soundUn;
    public Sprite musicNot;
    public Sprite musicUn;

    //Text musicText;
    // Text soundText;
    Text injuryText;
    Image soundImage;
    Image musicImage;
    //Button musicBtn;
    //Button soundBtn;
    Button injuryBtn;
    Button closeBtn;
    private Slider sliderMusic;
    private Slider sliderSound;

    public bool isInjury;
    public void Init()
    {
        //musicText = transform.Find("Music/Button/Text").GetComponent<Text>();
        //soundText = transform.Find("Sound/Button/Text").GetComponent<Text>();
        injuryText = transform.Find("Injury/Button/Text").GetComponent<Text>();
        musicImage = transform.Find("Music/Image").GetComponent<Image>();
        soundImage = transform.Find("Sound/Image").GetComponent<Image>();
        //musicBtn = transform.Find("Music/Button").GetComponent<Button>();
        //soundBtn = transform.Find("Sound/Button").GetComponent<Button>();
        injuryBtn = transform.Find("Injury/Button").GetComponent<Button>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        sliderMusic = transform.Find("Music/Slider").GetComponent<Slider>();
        sliderSound = transform.Find("Sound/Slider").GetComponent<Slider>();
        //musicBtn.onClick.AddListener(OpenMusic);
        sliderMusic.onValueChanged.AddListener(SliderMusic);
        sliderSound.onValueChanged.AddListener(SliderSound);
        //soundBtn.onClick.AddListener(OpenSound);
        injuryBtn.onClick.AddListener(OpeninjuryBtn);
        closeBtn.onClick.AddListener(Close);
        ExcelTool.LanguageEvent += CutLnag;
        //if (PlayerPrefs.GetString("Music") == "")
        //{
        //    musicImage.sprite = musicUn;
        //}
        //else
        //{
        //    musicImage.sprite = musicNot;
        //}
        SliderMusic(PlayerPrefs.GetFloat("SliderMusic", 1));
        SliderSound(AudioManager.Instance.soundValue);

        //if (PlayerPrefs.GetString("Sound") == "")
        //{
        //    soundImage.sprite = soundUn;
        //}
        //else
        //{
        //    soundImage.sprite = soundNot;
        //}
        isInjury = PlayerPrefs.GetString("Injury") != "";
    }

    private void SliderSound(float value)
    {
        sliderSound.value = value;
        AudioManager.Instance.RestoreSound(value);
        PlayerPrefs.SetFloat("SliderSound", value);
    }

    private void SliderMusic(float value)
    {
        sliderMusic.value = value;
        AudioManager.Instance.bgSource.volume = value;
        PlayerPrefs.SetFloat("SliderMusic", value);
    }
    private void CutLnag()
    {
        //if (PlayerPrefs.GetString("Music") == "")
        //{
        //    musicText.text = ExcelTool.lang["opening"];
        //}
        //else
        //{
        //    musicText.text = ExcelTool.lang["closing"];
        //}
        //if (PlayerPrefs.GetString("Sound") == "")
        //{
        //    soundText.text = ExcelTool.lang["opening"];
        //}
        //else
        //{
        //    soundText.text = ExcelTool.lang["closing"];
        //}
        if (PlayerPrefs.GetString("Injury") == "")
        {
            injuryText.text = ExcelTool.lang["opening"];
        }
        else
        {
            injuryText.text = ExcelTool.lang["closing"];
        }
    }

    void OpenMusic()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (PlayerPrefs.GetString("Music") == "")
        {
            musicImage.sprite = musicNot;
            //musicText.text = ExcelTool.lang["closing"];
            PlayerPrefs.SetString("Music", "pausemusic");
            AudioManager.Instance.bgSource.Pause();
        }
        else
        {
            musicImage.sprite = musicUn;
            //musicText.text = ExcelTool.lang["opening"];
            PlayerPrefs.DeleteKey("Music");
            AudioManager.Instance.bgSource.Play();
        }
    }

    void OpenSound()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (PlayerPrefs.GetString("Sound") == "")
        {
            soundImage.sprite = soundNot;
            //soundText.text = ExcelTool.lang["closing"];
            PlayerPrefs.SetString("Sound", "pausesound");
            AudioManager.Instance.isPause = true;
        }
        else
        {
            soundImage.sprite = soundUn;
            //soundText.text = ExcelTool.lang["opening"];
            PlayerPrefs.DeleteKey("Sound");
            AudioManager.Instance.isPause = false;
        }
    }

    void OpeninjuryBtn()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (isInjury)
        {
            isInjury = false;
            injuryText.text = ExcelTool.lang["opening"];
            PlayerPrefs.SetString("Injury", "");
        }
        else
        {
            isInjury = true;
            injuryText.text = ExcelTool.lang["closing"];
            PlayerPrefs.SetString("Injury", "injury");
        }
    }

    void Close()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        if (UIManager.Instance)
        {
            UIManager.Instance.DetectionPanel();
        }
    }
}
