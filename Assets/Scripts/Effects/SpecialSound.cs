using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSound : MonoBehaviour
{
    public string music;
    AudioSource source;
    float lastTime;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.enabled = false;
    }
    private void OnEnable()
    {
        lastTime = 0;
        source.enabled = true;
        AudioManager.Instance.PlaySource(music, source);
    }
    private void OnDisable()
    {
        source.enabled = false;
    }
    void Update()
    {
        if (source.enabled)
        {
            lastTime += Time.deltaTime;
            if ((lastTime >= 10) || AudioManager.Instance.isPause)
            {
                source.enabled = false;
                source.Stop();
            }
        }
    }

}
