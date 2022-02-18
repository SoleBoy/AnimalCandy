using UnityEngine;
using DG.Tweening;

public class CloudAnim : MonoBehaviour
{
    Transform cloudLeft;
    Transform cloudRight;
    private void Awake()
    {
        cloudLeft = transform.Find("Left");
        cloudRight = transform.Find("Right");
    }
    public void Cloud(bool isHide)
    {
        if (isHide)
        {
            cloudLeft.DOLocalMoveX(1f, 3);
            cloudRight.DOLocalMoveX(-11f, 3);
            Invoke("Buffer", 2.8f);
        }
        else
        {
            cloudLeft.DOLocalMoveX(-55f, 4);
            cloudRight.DOLocalMoveX(55f, 4);
        }
    }

    void Buffer()
    {
        cloudLeft.DOLocalMoveX(5f, 1);
        cloudRight.DOLocalMoveX(-15f, 1);
    }
}
