using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueAnimation : MonoBehaviour
{
    private float releaseTime;
    private int tipCount;

    private void OnEnable()
    {
        releaseTime = 1;
        OpenAnimal();
    }
    private void TimeAnimal()
    {
        tipCount = 1;
        StartCoroutine(PromptAnimal());
    }
    //提示动画
    public void OpenAnimal()
    {
        tipCount = 5;
        CancelInvoke();
        Invoke("TimeAnimal", releaseTime);
        transform.localScale = Vector3.one;
    }
    private IEnumerator PromptAnimal()
    {
        if (tipCount < 5)
        {
            transform.DOScale(Vector3.one * 1.5f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            transform.DOScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(0.5f);
            tipCount++;
            StartCoroutine(PromptAnimal());
        }
        else
        {
            releaseTime = UnityEngine.Random.Range(30, 60);
            OpenAnimal();
        }
    }
    private void OnDisable()
    {
        CancelInvoke();
        StopCoroutine("OpenAnimal");
    }
}
