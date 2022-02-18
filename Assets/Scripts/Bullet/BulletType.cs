using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    private RACEIMG state;

    private float nor_damage;

    private int objects_max;
    private ShooterItem bulletData;

    public void SetTarget(RACEIMG state, ShooterItem data, float damage, float distance)
    {
        this.state = state;
        bulletData = data;
        objects_max = 0;
        nor_damage = damage;
        switch (state)
        {
            case RACEIMG.Laser_4:
                GetComponent<Lase_Bullet>().OpenAnimal(distance);
                break;
            case RACEIMG.Magnet_11:
                GetComponent<Magnet_Bullet>().OpenAnimal(distance);
                break;
            case RACEIMG.Leeway_12:
                GetComponent<Leeway_Bullet>().OpenAnimal(distance);
                break;
            case RACEIMG.Sunfire_13:
                GetComponent<Sunfire_Bullet>().OpenAnimal(distance);
                break;
            case RACEIMG.Moon_16:
                GetComponent<Moon_Bullet>().OpenAnimal(distance);
                break;
            case RACEIMG.rod_17:
                GetComponent<Rod_Bullet>().OpenAnimal(distance);
                break;
            case RACEIMG.Flakes_18:
                GetComponent<Flakes_Bullet>().OpenAnimal(distance);
                break;
            case RACEIMG.Gasoline_19:
                GetComponent<Gasoline_Bullet>().OpenAnimal(distance);
                break;
            default:
                break;
        }
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
