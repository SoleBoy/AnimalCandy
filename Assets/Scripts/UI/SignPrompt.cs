using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SignPrompt : MonoBehaviour
{
    Text messText;
    Image elves;
    public Sprite[] sprites;
    AudioSource source;
    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        messText = transform.Find("Text").GetComponent<Text>();
        elves = transform.Find("Image").GetComponent<Image>();
    }
    /// <summary>
    /// 奖励提示
    /// </summary>
    /// <param name="mess"></param>
    /// <param name="index">0 金币 1 钻石 2 广告</param>
    public void TipMessage(float mess,int index)
    {
        AudioManager.Instance.PlaySource("detdiamond_1", source);
        elves.sprite = sprites[index];
        messText.text = "+" + mess;
        StartCoroutine(HideMess());
    }
    IEnumerator HideMess()
    {
        yield return new WaitForSeconds(2);
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
