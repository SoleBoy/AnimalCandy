using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalControl : MonoBehaviour
{
    public int indexAnimal;
    public bool isDeath;
    public Transform sizeScale;

    private Rigidbody body;
    private Collider box_Collider;
    private GameObject eyeEff;
    private AudioSource source;

    private float hight;
    private int layerMask;
    private void Awake()
    {
        if (!source)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
        eyeEff = transform.Find("eye").gameObject;
        body = GetComponent<Rigidbody>();
        box_Collider = GetComponent<BoxCollider>();
        sizeScale = transform.Find("Body");
        layerMask = LayerMask.GetMask("Enemy");
    }
    private void OnEnable()
    {
        body.isKinematic = true;
        isDeath = false;
        eyeEff.SetActive(false);
    }
    public void SetData(int index)
    {
        indexAnimal = index;
    }

    public void FreedObject(bool isHide)
    {
        body.useGravity = isHide;
        box_Collider.isTrigger = !isHide;
        if (isHide)
        {
            body.isKinematic = false;
        }
    }

    public void DestraeGame()
    {
        if (body.isKinematic) return;
        if (isDeath)
        {
            body.isKinematic = true;
            GameManager.Instance.CloneTip("+1");
            UIBase.Instance.RecycleAnimal(indexAnimal, transform);
            gameObject.SetActive(false);
            EffectGenerator.Instance.Meage(transform);
            EffectGenerator.Instance.EenemyPixel(transform, indexAnimal, 15);
            AudioManager.Instance.PlaySource(ExcelTool.Instance.sources[indexAnimal], source);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Roote"))
        {
            RemoveEnemy();
            return;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isDeath)
            {
                UIBase.Instance.Arrivals();
            }
            else if (collision.gameObject.GetComponent<AnimalControl>().isDeath)
            {
                JudeCollIsion();
            }
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            if (isDeath)
            {
                if (hight - transform.localPosition.y >= 5)
                {
                    RemoveEnemy();
                }
                else
                {
                    UIBase.Instance.guidePanel.OpenGuide(2, false);
                }
                UIBase.Instance.Arrivals();
            }
            else
            {
                JudeCollIsion();
            }
        }
        if (gameObject.activeSelf)
        {
            if (!eyeEff.activeInHierarchy)
            {
                StartCoroutine(HideEye());
            }
            AudioManager.Instance.PlaySource(ExcelTool.Instance.sources[indexAnimal], source);
        }
    }

    public void JudeCollIsion()
    {
        if(!isDeath)
        {
            isDeath = true;
            hight = transform.localPosition.y;
            UIBase.Instance.ImpactDetection();
            UIBase.Instance.JudeDetermine(transform);
            EffectGenerator.Instance.EenemyPixel(transform, indexAnimal, 6);
            EffectGenerator.Instance.Meage(transform);
        }
    }
    void RemoveEnemy()
    {
        gameObject.SetActive(false);
        body.isKinematic = true;
        UIBase.Instance.DeathRecovery(transform, indexAnimal);
        EffectGenerator.Instance.RemoveGhost(transform);
        EffectGenerator.Instance.Meage(transform);
        EffectGenerator.Instance.EenemyPixel(transform, indexAnimal, 15);
    }


    IEnumerator HideEye()
    {
        eyeEff.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        eyeEff.SetActive(false);
    }
}
