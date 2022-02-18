using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SkillRewards : MonoBehaviour
{
    public Mesh[] meshes;
    Animator beam;
    Transform villain;
    GameObject time1;
    GameObject time2;
    GameObject time3;
    GameObject TipCon;
    GameObject textTip;

    float lastTime;
    float sumTime;
    bool isRewards;

    void Awake()
    {
        beam = transform.Find("Beam").GetComponent<Animator>();
        villain = transform.Find("Villain");
        time1 = transform.Find("NumBer/Time1").gameObject;
        time2 = transform.Find("NumBer/Time2").gameObject;
        time3 = transform.Find("NumBer/Time3").gameObject;
        TipCon = transform.Find("Tip").gameObject;
        textTip = transform.Find("Canvas").gameObject;
        sumTime = PlayerPrefs.GetFloat("SkillTime",100);
        BarText((int)sumTime);
        if (sumTime < 0)
        {
            isRewards = true;
            beam.gameObject.SetActive(true);
            beam.Play("MixEff", 0, 0);
            StartCoroutine(VillainAnim());
        }
        if(GameManager.Instance.modeSelection == "roude")
        {
            villain.localEulerAngles = new Vector3(155,0,0);
            TipCon.transform.localPosition = new Vector3(0,2,0.4f);
            transform.Find("NumBer").localEulerAngles = new Vector3(35, 0, 0);
        }
    }

    private void BarText(int num)
    {
        if (num >= 100)
        {
            time1.GetComponent<MeshFilter>().mesh = meshes[num/100];
            time2.GetComponent<MeshFilter>().mesh = meshes[num / 10 % 10];
            time3.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
        }
        else if (num >= 10 && num < 100)
        {
            time1.GetComponent<MeshFilter>().mesh = null;
            time2.GetComponent<MeshFilter>().mesh = meshes[num / 10 % 10];
            time3.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
        }
        else if (num < 10 && num > 0)
        {
            time1.GetComponent<MeshFilter>().mesh = null;
            time2.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
            time3.GetComponent<MeshFilter>().mesh = null;
        }
        else
        {
            time1.GetComponent<MeshFilter>().mesh = null;
            time2.GetComponent<MeshFilter>().mesh = null;
            time3.GetComponent<MeshFilter>().mesh = null;
        }
    }
    void Update()
    {
        if (UIManager.Instance.isTime) return;
        if(!isRewards)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 1)
            {
                sumTime -= lastTime;
                BarText((int)sumTime);
                lastTime = 0;
                if (sumTime <= 0)
                {
                    sumTime = -10;
                    isRewards = true;
                    beam.gameObject.SetActive(true);
                    beam.Play("MixEff",0,0);
                    StartCoroutine(VillainAnim());
                }
                PlayerPrefs.SetFloat("SkillTime", sumTime);
            }
        }
    }
    private void OnMouseDown()
    {
        if (isRewards && !UIManager.Instance.isTime)
        {
            UIManager.Instance.rewardsPanel.RandomSkills();
        }
    }
    public void FlyOut()
    {
        isRewards = false;
        sumTime = 100;
        BarText((int)sumTime);
        PlayerPrefs.SetFloat("SkillTime", sumTime);
        villain.DOLocalMoveY(15, 1);
        TipCon.gameObject.SetActive(false);
        textTip.SetActive(false);
    }

    IEnumerator VillainAnim()
    {
        villain.DOLocalMoveY(1, 1);
        yield return new WaitForSeconds(1);
        TipCon.gameObject.SetActive(true);
        textTip.SetActive(true);
        villain.DOScale(new Vector3(60,216,48),0.25f);
        yield return new WaitForSeconds(0.25f);
        villain.DOScale(new Vector3(60,180, 60), 0.25f);
        yield return new WaitForSeconds(0.25f);
        UIManager.Instance.guidePanel.OpenGuide(8,false);
        StartCoroutine(VillainIdle());
    }

    //休息动画 
    IEnumerator VillainIdle()
    {
        villain.DOLocalMoveY(1.5f, 0.4f);
        yield return new WaitForSeconds(0.4f);
        villain.DOLocalMoveY(1, 0.4f);
        yield return new WaitForSeconds(0.4f);
        if (isRewards)
            StartCoroutine(VillainIdle());
    }
}
