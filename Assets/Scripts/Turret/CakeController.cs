using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeController : MonoBehaviour
{
    public Mesh[] meshes;
    public CakeHealth[] cakeHealths;
    public Transform water;
    public bool isvictory;

    private GameObject cubePrefab;

    private float hp_curr;
    private float hp_max;
    private int hp_multiple;
    private float max_HP;
    private float sou_Max = 0;
    private Vector3 point;
    private EnemyControl enemySoul;
    private bool isDeath;
    private bool isRise;
    private void Start()
    {
        cubePrefab = Resources.Load<GameObject>("Effects/xin");
    }
    public void BarMultiple(int geade)
    {
        max_HP += 100;
        UIManager.Instance.CurretHp(max_HP);
        for (int i = 0; i < cakeHealths.Length; i++)
        {
            cakeHealths[i].BarMultiple(meshes, geade);
        }
    }

    public void SetHealth()
    {
        for (int i = 0; i < cakeHealths.Length; i++)
        {
            cakeHealths[i].BarText(meshes,100);
            cakeHealths[i].BarMultiple(meshes, UIManager.Instance.turretPanel.cakeGrade);
        }
        hp_curr = 100;
        hp_max = 100;
        hp_multiple = 1;
        isRise = true;
        isDeath = false;
        isvictory = false;
        max_HP = UIManager.Instance.turretPanel.cakeGrade * 100;
        UIManager.Instance.CurretHp(max_HP);
    }
    //复活血量
    public void RiseHealth()
    {
        hp_curr = Mathf.Clamp(hp_curr,UIManager.Instance.turretPanel.cakeGrade * 100 * 0.5f,100);
        max_HP = hp_curr;
        UIManager.Instance.CurretHp(max_HP);
        for (int i = 0; i < cakeHealths.Length; i++)
        {
            cakeHealths[i].BarText(meshes,(int)hp_curr);
            cakeHealths[i].BarMultiple(meshes,1);
        }
        isDeath = false;
        isvictory = false;
        if (enemySoul != null)
        {
            SoulRecycling(true);
        }
    }
    //取消复活
    public void CancelRise()
    {
        StartCoroutine(DelyLevel());
    }
    //剩余血量
    public string Remaining()
    {
        return max_HP.ToString();
    }
    //满血过关？
    public bool FullBlood()
    {
        if(hp_curr >= hp_max && hp_multiple==1)
        {
            return true;
        }
        return false;
    }

    private void AddBruise(float harm)
    {
        hp_curr += harm;
        max_HP = Mathf.Clamp(max_HP,1, max_HP + harm);
        UIManager.Instance.CurretHp(max_HP);
        if (hp_multiple == 1 && hp_curr > 100)
        {
            hp_curr = 100;
        }
        else if (hp_curr > 100)
        {
            hp_curr = hp_curr - 100;
            hp_multiple -= 1;
            for (int i = 0; i < cakeHealths.Length; i++)
            {
                cakeHealths[i].BarText(meshes, 100);
                cakeHealths[i].BarMultiple(meshes, UIManager.Instance.turretPanel.cakeGrade - (hp_multiple-1));
            }
        }
        for (int i = 0; i < cakeHealths.Length; i++)
        {
            cakeHealths[i].BarText(meshes, (int)hp_curr);
        };
    }

    private void OnBruise(float harm)
    {
        if (hp_curr >= 0)
        {
            if(CreateModel.Instance.level == 0)
            {
                hp_curr = Mathf.Clamp(hp_curr,1, hp_curr-1);
                max_HP -= 1;
            }
            else
            {
                hp_curr -= harm;
                max_HP -= harm;
            }
            UIManager.Instance.CurretHp(max_HP);
            if (hp_curr <= 0 && !isDeath)
            {
                NextGame();
            }
            else
            {
                for (int i = 0; i < cakeHealths.Length; i++)
                {
                    cakeHealths[i].BarText(meshes, (int)hp_curr);
                }
                AudioManager.Instance.PlayTouch("cakehurt_1");
            }
        }
    }
    private void NextGame()
    {
        if (UIManager.Instance.turretPanel.cakeGrade - hp_multiple > 0)
        {
            for (int i = 0; i < cakeHealths.Length; i++)
            {
                cakeHealths[i].BarMultiple(meshes, (int)(UIManager.Instance.turretPanel.cakeGrade - hp_multiple));
            }
            hp_multiple += 1;
            hp_curr += hp_max;
            if (hp_curr <= 0)
            {
                NextGame();
            }
        }
        else
        {
            if (isRise)
            {
                isRise = false;
                UIManager.Instance.risePanel.OpenPanel();
            }
            else
            {
                isDeath = true;
                StartCoroutine(DelyLevel());
            }
        }
        for (int i = 0; i < cakeHealths.Length; i++)
        {
            cakeHealths[i].BarText(meshes, (int)hp_curr);
        }
    }

    public IEnumerator DelyLevel()
    {
        AudioManager.Instance.PlayTouch("cakedead_1");
        SoulRecycling(false);
        WaterAnim(true);
        yield return new WaitForSeconds(2);
        isvictory = true;
        UIManager.Instance.isTime = true;
        UIManager.Instance.settlePanel.OpenSettle(false);
        yield return new WaitForSeconds(0.2f);
        CreateModel.Instance.GlobalDestruction();
    }
    private IEnumerator CreateEffects(Vector3 pos, float dely)
    {
        yield return new WaitForSeconds(dely);
        float ran = Random.Range(-1f, 1f);
        pos.x += ran;
        var go = ObjectPool.Instance.CreateObject(cubePrefab.name, cubePrefab);
        go.transform.localPosition = pos;
        go.GetComponent<EndCubeEff>().enabled = true;
    }
    public void TriggerEnemy(Transform other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            CreateModel.Instance.enemys.Remove(other);
            other.DOScale(Vector3.zero, 1f);
            StartCoroutine(ObjectNarrow(other.GetComponent<EnemyControl>(),null));
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            other.gameObject.SetActive(false);
            CreateModel.Instance.enemyBullets.Remove(other);
            StartCoroutine(ObjectNarrow(null, other.GetComponent<EnemyBullet>()));
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<Collider>().enabled = false;
            AddBruise(other.transform.parent.GetComponent<SkillSnowman>().heartNum);
            for (int i = 0; i < cakeHealths.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    StartCoroutine(RestoreHp(j * 0.3f, cakeHealths[i].transform.position));
                }
            }
        }
    }
    IEnumerator ObjectNarrow(EnemyControl enemy, EnemyBullet bullet)
    {
        yield return new WaitForSeconds(1f);
        enemySoul = enemy;
        if (enemy)
        {
            point = enemy.transform.position;
            sou_Max = enemy.min_attack;
        }
        else
        {
            point = bullet.transform.position;
            sou_Max = bullet.shoot_hurt;
        }
        if(!UIManager.Instance.isTime)
        {
            OnBruise(sou_Max);
        }
        if (hp_curr >= 1)
        {
            SoulRecycling(true);
        }
        else if (CreateModel.Instance.enemys.Count > 1)
        {
            SoulRecycling(true);
        }
    }
    private void SoulRecycling(bool isSoul)
    {
        if (isSoul)
        {
            sou_Max = Mathf.Clamp(sou_Max, 1, 10);
            for (int i = 0; i < sou_Max; i++)
            {
                StartCoroutine(CreateEffects(point, 0.2f * i));
            }
            if (enemySoul != null)
            {
                CreateModel.Instance.LevelControl(enemySoul.enemyData.soul, false);
                enemySoul.gameObject.SetActive(false);
            }
            if (!UIManager.Instance.skillFirst && CreateModel.Instance.enemys.Count >= 1)
            {
                CreateModel.Instance.sendTroops = true;
                UIManager.Instance.GuideInfo(ExcelTool.lang["tiproude4"]);
            }
        }
        else
        {
            for (int i = 0; i < CreateModel.Instance.enemys.Count; i++)
            {
                TurretDrag.Instance.EenemyPixel(CreateModel.Instance.enemys[i].gameObject,
                    CreateModel.Instance.enemys[i].GetComponent<EnemyControl>().enemyID, false);
                ObjectPool.Instance.CollectObject(CreateModel.Instance.enemys[i].gameObject);
            }
            CreateModel.Instance.enemys.Clear();
        }
        enemySoul = null;
    }
    private IEnumerator RestoreHp(float dely,Vector3 point)
    {
        yield return new WaitForSeconds(dely);
        float ran = Random.Range(-1f, 1f);
        point.y = 35;
        var go = ObjectPool.Instance.CreateObject(cubePrefab.name, cubePrefab);
        go.GetComponent<EndCubeEff>().enabled = false;
        go.transform.localPosition = point;
        go.transform.DOLocalMoveY(0,1);
        yield return new WaitForSeconds(1);
        go.SetActive(false);
    }

    //水动画
    public void WaterAnim(bool isHide)
    {
        if (isHide)
        {
            water.gameObject.SetActive(true);
            water.DOLocalMoveY(15f, 1.5f);
        }
        else
        {
            water.DOLocalMoveY(-6, 5);
            Invoke("HideWater", 2);
        }
    }
    void HideWater()
    {
        water.gameObject.SetActive(false);
    }
}
