using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraView : MonoBehaviour
{
    public RectTransform leftUI;
    public RectTransform rightUI;

    private Vector3 entPoint;
    private Vector3 entEngle;
    private Vector3 starPoint = new Vector3(0, 7, -45); //0 18 36.5 0 75 0
    private Vector3 starEngle = new Vector3(10, 0, 0);

    // 正交 0 28 25 55 0 0  13
    private void Start()
    {
        if(GameManager.Instance.modeSelection == "roude")
        {
            //entPoint = new Vector3(0, 15, 35);
            entEngle = new Vector3(65, 0, 0);
            if (GameManager.Instance.sceneIndex == 0)
            {
                entPoint = new Vector3(0, 22, 31.5f);
            }
            else if (GameManager.Instance.sceneIndex == 1)
            {
                entPoint = new Vector3(0, 18.5f, 33.3f);
            }
            else if (GameManager.Instance.sceneIndex == 2)
            {
                entPoint = new Vector3(0, 17.5f, 35);
            }
            else
            {
                leftUI.anchoredPosition3D = new Vector3(50, -80, 0);
                rightUI.anchoredPosition3D = new Vector3(-50, -80, 0);
                entPoint = new Vector3(0, 16f, 37f);

            }
        }
        else
        {
            entEngle = new Vector3(35, 0, 0);
            if (GameManager.Instance.sceneIndex == 0)
            {
                entPoint = new Vector3(0, 17.2f, -21f);
            }
            else if (GameManager.Instance.sceneIndex == 1)
            {
                entPoint = new Vector3(0, 14.5f, -17.5f);
            }
            else if (GameManager.Instance.sceneIndex == 2)
            {
                entPoint = new Vector3(0, 14.5f, -15.5f);
            }
            else
            {
                leftUI.anchoredPosition3D = new Vector3(50, -80, 0);
                rightUI.anchoredPosition3D = new Vector3(-50, -80, 0);
                entPoint = new Vector3(0, 16.5f, -14.5f);

            }
            StartCoroutine(CameraMove(entPoint.y, entPoint.z));
        }
    }
    public void InitMove()
    {
        transform.localPosition = starPoint;
        transform.localEulerAngles = starEngle;
        StartCoroutine(CameraMove(entPoint.y, entPoint.z));
    }

    IEnumerator CameraMove(float y, float z)
    {
        transform.DOLocalMoveZ(z, 1);
        yield return new WaitForSeconds(1);
        transform.DOLocalMoveY(y, 1);
        transform.DOLocalRotate(entEngle, 1);
    }
}
