using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ArmsPanel : MonoBehaviour
{
    private Image[] images;
    private Text levelText;
    private float hight;
    private float timeCount;
    private Transform parent;
    private Transform boosTip;
    private Image hideMask;
    public void Init(int index)
    {
        timeCount = index;
        hight = Screen.height*0.5f+150;
        parent = transform.Find("Prent");
        hideMask = GetComponent<Image>();
        boosTip=transform.Find("Prent/Head/BoosText");
        Transform headPrent = transform.Find("Prent/Head");
        images = new Image[headPrent.childCount-1];
        for (int i = 0; i < headPrent.childCount-1; i++)
        {
            images[i] = headPrent.GetChild(i).GetComponent<Image>();
        }
        headPrent = null;
        levelText = transform.Find("Prent/Text").GetComponent<Text>();
    }

    public void SetArmsInfo(List<Sprite> sprites,float level,bool isBoos)
    {
        parent.localPosition = Vector3.up * hight;
        levelText.text = string.Format("{0}:{1}",ExcelTool.lang["pass"],level);
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < sprites.Count; i++)
        {
            if (i < images.Length)
            {
                images[i].sprite = sprites[i];
                images[i].SetNativeSize();
                images[i].gameObject.SetActive(true);
            }
        }
        boosTip.gameObject.SetActive(false);
        if (isBoos && sprites.Count <= images.Length)
        {
            Vector3 point = images[sprites.Count-1].transform.localPosition;
            point.y += 34;
            boosTip.localPosition = point;
            boosTip.gameObject.SetActive(true);
        }
        hideMask.DOFade(0,0.5f);
        gameObject.SetActive(true);
        StartCoroutine(Animator());
    }

    IEnumerator Animator()
    {
        yield return new WaitForSeconds(timeCount);
        hideMask.DOFade(0.6f, 0.5f);
        parent.DOLocalMoveY(0,0.5f);
        yield return new WaitForSeconds(2.5f);
        parent.DOLocalMoveY(-hight, 0.5f);
        hideMask.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
