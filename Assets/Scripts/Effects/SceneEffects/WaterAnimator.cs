using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterAnimator : MonoBehaviour, WeatherMaager
{
    public float maxY;
    bool isWater = true;
    //WaitForSeconds wait = new WaitForSeconds(15);
    Vector3 point;
    private void Awake()
    {
        point = transform.localPosition;
    }

    public void Weather(bool isHide,bool hide)
    {
        if(gameObject.activeInHierarchy)
        {
            isWater = isHide;
            if (hide)
            {
                StopCoroutine(WaterAnim());
                gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (isWater)
        {
            isWater = false;
            transform.localPosition = point;
            StartCoroutine(WaterAnim());
        }
    }
    IEnumerator WaterAnim()
    {
        transform.DOLocalMoveY(maxY, 10);
        yield return new WaitForSeconds(25);
        transform.DOLocalMoveY(-10,10);
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
        isWater = true;
    }

}
