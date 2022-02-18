using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private RACEIMG state;

    private float nor_damage;
    private float distance_max;
    private float distanceToTarget;//与目标初始距离
    private float distance;//与目标实时距离
    private Vector3 targetPos;//目标位置
    private Vector3 selfPos;//初始位置点
    private ShooterItem bulletData;
    private GameObject effectPrefab;

    private float objects_max=0;
    private float bulletSpeed;

    public void SetTarget(RACEIMG state,Vector3 _target,ShooterItem data,float damage)
    {
        this.state = state;
        selfPos = transform.position;
        targetPos = _target;
        bulletData = data;
        objects_max = 0;
        nor_damage = damage;
        distanceToTarget = Vector3.Distance(transform.position, targetPos);
        if (bulletData.speed != "-1")
        {
            if (GameManager.Instance.modeSelection == "roude")
            {
                if (state == RACEIMG.Rem_0)
                {
                    Vector3 vector = Vector3.one;
                    vector.z = 0.2f;
                    transform.localScale = vector;
                } 
                bulletSpeed = float.Parse(bulletData.speed.Split('|')[1]);
                distance_max = float.Parse(bulletData.atk_distance.Split('|')[1]);
            }
            else
            {
                distance_max = float.Parse(bulletData.atk_distance.Split('|')[0]);
                bulletSpeed = float.Parse(bulletData.speed.Split('|')[0]);
            }
        }
        switch (state)
        {
            case RACEIMG.Fire_3:
            case RACEIMG.Frozen_6:
            case RACEIMG.Snow_9:
            case RACEIMG.Leaf_10:
            case RACEIMG.Cluster_15:
                if (!effectPrefab)
                {
                    effectPrefab = Resources.Load<GameObject>("Effects/Candy_Effects"+(int)state);
                }
                break;
            case RACEIMG.Rockets_20:
                Vector3 angle=transform.localEulerAngles;
                angle.x = 0;
                transform.localEulerAngles = angle;
                GetComponent<Rockets_Bullet>().OpenAnimal(distance_max);
                break;
            default:
                break;
        }
    }
    void Update()
    {
        if (UIManager.Instance.isTime) return;
        if (gameObject.activeInHierarchy)
        {
            switch (state)
            {
                case RACEIMG.Rem_0:
                case RACEIMG.Egg_5:
                case RACEIMG.Frozen_6:
                case RACEIMG.Cameo_7:
                case RACEIMG.Leaf_10:
                case RACEIMG.Icecone_14:
                case RACEIMG.Cluster_15:
                case RACEIMG.Rockets_20:
                    GeneralAttack();
                    break;
                case RACEIMG.Bane_1:
                case RACEIMG.Ice_2:
                case RACEIMG.Fire_3:
                case RACEIMG.Snow_9:
                    Shoot();
                    break;
                default:
                    break;
            }
        }
    }
    //抛物线运动
    void Shoot()
    {
        distance = Vector3.Distance(transform.localPosition, targetPos);
        if (distance <= 0.15f && objects_max <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.LookAt(targetPos);
            float angle = Mathf.Min(1, distance / distanceToTarget) * 45;
            transform.rotation = transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            transform.Translate(Vector3.forward * Mathf.Min(bulletSpeed * Time.deltaTime, distance));
        }
    }
    void GeneralAttack()
    {
        //if (state == RACEIMG.Cameo_7 && objects_max > 0) return;
        if (distance_max >= Vector3.Distance(transform.position,selfPos))
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
        else
        {
            if (state == RACEIMG.Frozen_6)
            {
                gameObject.SetActive(false);
            }
            else if (state == RACEIMG.Rockets_20)
            {
                distance_max = 10000;
                GetComponent<Rockets_Bullet>().StopAnimal();
            }
            else if(objects_max <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        //transform.LookAt(targetPos);
        //transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
    //紫火炮
    void CreateFire(Vector3 pos)
    {
        var effect = ObjectPool.Instance.CreateObject(effectPrefab.name, effectPrefab);
        effect.transform.localPosition = pos;
        effect.transform.localEulerAngles = Vector3.zero;
        effect.GetComponent<Fire_Bullet>().CreateEffects(bulletData, nor_damage);
        gameObject.SetActive(false);
    }
    //冰冻糖果
    void CreateFrozen(Vector3 pos)
    {
        int ran = UnityEngine.Random.Range(1,4);
        for (int i = 0; i < ran; i++)
        {
            pos.x += UnityEngine.Random.Range(-1f, 1f);
            var effect = ObjectPool.Instance.CreateObject(effectPrefab.name, effectPrefab);
            effect.transform.localPosition = pos;
            effect.transform.localEulerAngles = Vector3.zero;
            effect.GetComponent<Frozen_Bullet>().InitState(pos);
        }
    }
    //雪崩
    void CreateSnow(Vector3 pos)
    {
        pos.y += 15;
        var effect = ObjectPool.Instance.CreateObject(effectPrefab.name, effectPrefab);
        effect.transform.localPosition = pos;
        effect.transform.localEulerAngles = Vector3.zero;
        effect.GetComponent<Snow_Bullet>().SetHurt(state,bulletData, nor_damage);
        gameObject.SetActive(false);
    }
    //叶子石壁
    void CreateLeaf(Vector3 pos)
    {
        var effect = ObjectPool.Instance.CreateObject(effectPrefab.name, effectPrefab);
        effect.transform.localPosition = pos;
        effect.transform.localEulerAngles = Vector3.zero;
        effect.GetComponent<LeafBullet>().SetHurt(state, bulletData, nor_damage);
        gameObject.SetActive(false);
    }
    //蘑菇爆炸
    void CreateCluster(Vector3 pos)
    {
        pos.y += 1;
           var effect = ObjectPool.Instance.CreateObject(effectPrefab.name, effectPrefab);
        effect.transform.localPosition = pos;
        effect.transform.localEulerAngles = Vector3.zero;
        effect.GetComponent<Cluster_Bullet>().OpenAnimal();
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
            BulletEffects(other.transform);
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            BulletEffects(other.transform);
        }
    }
    
    private void BulletEffects(Transform other)
    {
        if (state == RACEIMG.Frozen_6)
        {
            CreateFrozen(other.position);
        }
        else
        {
            if (objects_max == 0)
            {
                switch (state)
                {
                    case RACEIMG.Rem_0:
                        gameObject.SetActive(false);
                        break;
                    case RACEIMG.Ice_2:
                    case RACEIMG.Bane_1:
                        GetComponent<BulletAnimal>().OpenAnimal();
                        break;
                    case RACEIMG.Fire_3:
                        CreateFire(other.position);
                        break;
                    case RACEIMG.Egg_5:
                        GetComponent<Egg_Bullet>().OpenAnimal();
                        break;
                    case RACEIMG.Cameo_7:
                        GetComponent<MagicGem_Animal>().OpenAnimal();
                        break;
                    case RACEIMG.Snow_9:
                        CreateSnow(other.position);
                        break;
                    case RACEIMG.Leaf_10:
                        CreateLeaf(other.position);
                        break;
                    case RACEIMG.Icecone_14:
                        GetComponent<Icecone_Bullet>().OpenAnimal();
                        break;
                    case RACEIMG.Cluster_15:
                        CreateCluster(other.position);
                        break;
                    default:
                        break;
                }
            }
        }
        objects_max++;
    }
}
