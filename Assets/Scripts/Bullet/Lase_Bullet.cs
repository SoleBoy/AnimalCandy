using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lase_Bullet : MonoBehaviour
{
    private Transform laserAnim;
    private LineRenderer lineRenderer;
    private bool isOpen;
    private void Awake()
    {
        isOpen = true;
        laserAnim = transform.Find("LaserAnim");
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void OpenAnimal(float distance)
    {
        if(isOpen)
        {
            isOpen = false;
            JudeBox(distance);
        }
        StartCoroutine(HideLaster());
    }
    IEnumerator HideLaster()
    {
        laserAnim.DOScaleX(0.3f,0.2f);
        laserAnim.DOScaleZ(0.3f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        laserAnim.DOScaleX(0.5f, 0.2f);
        laserAnim.DOScaleZ(0.5f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        laserAnim.DOScaleX(0, 0.1f);
        laserAnim.DOScaleZ(0, 0.1f);
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
    void JudeBox(float distance)
    {
        lineRenderer.SetPosition(1,new Vector3(0,0,distance));
        lineRenderer.gameObject.AddComponent<BoxCollider>().isTrigger=true;
        laserAnim.localPosition = new Vector3(0,0,distance*0.5f);
        laserAnim.localScale = new Vector3(0, distance*0.5f, 0);
    }
}
