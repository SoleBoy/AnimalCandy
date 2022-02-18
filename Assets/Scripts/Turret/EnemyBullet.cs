using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBullet : MonoBehaviour
{
    private Collider boxcollider;
    private Vector3 endPoint;

    private float speed;
    private float distance;
    private float distanceToTarget;
    private float shoot_type;
    public float shoot_hurt;
    private Vector3 vector;
    private void Awake()
    {
        boxcollider = GetComponent<Collider>();
        vector = transform.localScale;
    }

    public void SetInit(Vector3 point,float speed,float shoot_type,float hurt,Color color)
    {
        this.speed = speed;
        this.shoot_type = shoot_type;
        this.shoot_hurt = hurt;
        this.endPoint = point;
        distanceToTarget = Vector3.Distance(transform.position, endPoint);
        boxcollider.enabled = true;
    }
    private void Update()
    {
        if (UIManager.Instance.isTime) return;
        if (gameObject.activeInHierarchy && boxcollider.enabled)
        {
            distance = Vector3.Distance(transform.localPosition, endPoint);
            if(distance <= 0.02f)
            {
                gameObject.SetActive(false);
                boxcollider.enabled = false;
                CreateModel.Instance.enemyBullets.Remove(transform);
            }
            else
            {
                if (shoot_type == 1)
                {
                    GeneralAttack();
                }
                else
                {
                    Shoot();
                }
            }
        }
    }
    //抛物线运动
    void Shoot()
    {
        transform.LookAt(endPoint);
        float angle = Mathf.Min(1, distance / distanceToTarget) * 35;
        transform.rotation = transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
        transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, distance));
    }

    void GeneralAttack()
    {
        transform.LookAt(endPoint);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            boxcollider.enabled = false;
            transform.DOScale(vector * 1.5f,0.3f);
            Invoke("HideGame",0.3f);
            CreateModel.Instance.enemyBullets.Remove(transform);
        }
    }

    void HideGame()
    {
        gameObject.SetActive(false);
    }
}
