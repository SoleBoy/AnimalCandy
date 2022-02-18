using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Fire_Bullet : MonoBehaviour
{
    private AudioSource source;
    private Transform effectHalo;
    private Material material_fire;
    private Material material_halo;

    private ShooterItem item;
    private float nor_damage;
    private float objects_max;

    private Color color;
    private void Awake()
    {
        effectHalo = transform.Find("Halo");
        material_halo = transform.Find("Halo/Cube").GetComponent<Renderer>().material;
        material_fire = transform.GetComponent<Renderer>().material;
        color = material_fire.color;
        if (!source) source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void CreateEffects(ShooterItem item, float nor)
    {
        AudioManager.Instance.PlaySource("bulletbown_4", source);
        this.item = item;
        nor_damage = nor;
        objects_max = 0;
        StartCoroutine(HaloFire());
        StartCoroutine(HideEffects());
    }

    IEnumerator HideEffects()
    {
        material_fire.DOFade(1,0.2f);
        transform.DOScale(new Vector3(120, 120, 120), 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.DOScale(new Vector3(150, 150, 150), 1f);
        material_fire.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        transform.localScale = Vector3.zero;
        ObjectPool.Instance.CollectObject(gameObject);
    }

    IEnumerator HaloFire()
    {
        effectHalo.DOScale(Vector3.one * 0.8f, 0.2f);
        material_halo.DOFade(0.7f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        effectHalo.DOScale(Vector3.one * 1.5f, 0.8f);
        material_halo.DOFade(0f, 0.8f);
        yield return new WaitForSeconds(0.8f);
        effectHalo.localScale = Vector3.zero;
        material_halo.DOFade(1f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && objects_max < item.dam_max)
        {
            if (objects_max < item.dam_max)
            {
                if (GameManager.Instance.modeSelection == "roude")
                {
                    other.GetComponent<EnemyControl>().InjuredType(item.buff, item.type, item.param_line, color, nor_damage);
                }
                else
                {
                    other.GetComponent<EnemyControl>().InjuredType(item.buff, item.type, item.param, color, nor_damage);
                }
            }
            objects_max++;
        }
    }
}
