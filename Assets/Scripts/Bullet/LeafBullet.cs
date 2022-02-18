using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LeafBullet : MonoBehaviour
{
    private Material material;
    private Color color;

    private RACEIMG state;
    private ShooterItem bulletData;

    private float nor_damage;
    private float objects_max = 0;
    private void Awake()
    {
        material = transform.Find("Cube").GetComponent<Renderer>().material;
        color = material.color;
        color.a = 0.5f;
    }

    public void SetHurt(RACEIMG state, ShooterItem item, float nor)
    {
        bulletData = item;
        this.state = state;
        objects_max = 0;
        nor_damage = nor;
        material.color = color;
        transform.localScale = Vector3.zero;
        StartCoroutine(SnowAnimator());
    }

    IEnumerator SnowAnimator()
    {
        transform.DOScale(Vector3.one * 2, 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.DOScale(Vector3.one * 3, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOScale(Vector3.one * 4, 0.5f);
        material.DOFade(0, 0.5f);
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
