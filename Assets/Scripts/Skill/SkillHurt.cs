using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHurt : MonoBehaviour
{
    public Color color;
  
    private float hurtNum;
    private float hurt_max;
    private SkillItem skillitem;

    public void SetInit(SkillItem item, float Hurt)
    {
        skillitem = item;
        hurt_max = Hurt;
        if(item.dam_max != "max")
        {
            hurtNum = float.Parse(item.dam_max);
        }
        else
        {
            hurtNum = -10;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnHurt(other.GetComponent<EnemyControl>());
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            OnHurt(other.gameObject.GetComponent<EnemyControl>());
        }
    }
    private void OnHurt(EnemyControl enemyControl)
    {
        if (hurtNum != -10 && hurtNum >= 0)
        {
            hurtNum--;
            if (hurtNum < 0)
            {
                return;
            }
        }
        float basisHurt = 0;
        if (enemyControl.isBoos)
        {
            basisHurt = skillitem.boss_atk_percentage * 0.01f * enemyControl.maxHp + hurt_max;
        }
        else
        {
            basisHurt = skillitem.atk_percentage * 0.01f * enemyControl.maxHp + hurt_max;
        }
        if (GameManager.Instance.modeSelection == "roude")
        {
            enemyControl.InjuredType(skillitem.buff_id, skillitem.type, skillitem.param_line, color, basisHurt);
        }
        else
        {
            enemyControl.InjuredType(skillitem.buff_id, skillitem.type, skillitem.param, color, basisHurt);
        }
    }
}
