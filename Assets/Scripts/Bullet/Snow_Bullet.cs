using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Snow_Bullet : MonoBehaviour
{
    private Transform snow_block;
    private Material material;
    private Color color;
    private Collider box_collider;
    private Vector3 sizeScale;
    private Vector3 sizePoint;

    private RACEIMG state;
    private ShooterItem bulletData;

    private float nor_damage;
    private float objects_max = 0;
    private void Awake()
    {
        snow_block = transform.Find("Snow");
        material = snow_block.GetComponent<Renderer>().material;
        color = material.color;
        box_collider = GetComponent<Collider>();
        sizeScale = snow_block.localScale;
    }

    public void SetHurt(RACEIMG state, ShooterItem item, float nor)
    {
        bulletData = item;
        this.state = state;
        objects_max = 0;
        nor_damage = nor;
        material.color = color;
        snow_block.localScale = Vector3.zero;
        sizePoint = transform.localPosition;
        StartCoroutine(SnowAnimator());
    }

    IEnumerator SnowAnimator()
    {
        snow_block.DOScale(sizeScale,0.2f);
        transform.DOLocalMoveY(sizePoint.y-15, 0.2f);
        yield return new WaitForSeconds(0.2f);
        box_collider.enabled = true;
        snow_block.DOScale(sizeScale*1.2f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        box_collider.enabled = false;
        snow_block.DOScale(Vector3.zero, 0.5f);
        material.DOFade(0.1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && objects_max < bulletData.dam_max)
        {
            if (objects_max < bulletData.dam_max)
            {
                if (GameManager.Instance.modeSelection == "roude")
                {
                    other.GetComponent<EnemyControl>().InjuredType(bulletData.buff, bulletData.type, bulletData.param_line, TurretDrag.Instance.turrets[(int)state], nor_damage);
                }
                else
                {
                    other.GetComponent<EnemyControl>().InjuredType(bulletData.buff, bulletData.type, bulletData.param, TurretDrag.Instance.turrets[(int)state], nor_damage);
                }
            }
            objects_max++;
        }
    }

}
