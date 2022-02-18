using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicGem_Animal : MonoBehaviour
{

    public void OpenAnimal()
    {
        StartCoroutine(Animal());
    }
    IEnumerator Animal()
    {
        transform.DOScale(Vector3.one*1.2f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.DOScale(Vector3.one * 1.3f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        transform.DOScale(Vector3.one * 1.4f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        transform.localScale=Vector3.one;
        gameObject.SetActive(false);
    }
}
