using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    private static BaseHealth instance;
    public static BaseHealth Instance { get => instance; }
    public Mesh[] meshes;
    public Transform water;
    GameObject cubePrefab;
    Transform endCube;
    GameObject multiple1;
    GameObject multiple2;

    GameObject hpText1;
    GameObject hpText2;
    GameObject hpText3;

    public float hp=100;
    private float maxHp;
    private float max_HP;
    private int tubeHp=1;
    public bool isOver;

    private Vector3 scaleEnd;
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }
    void Start()
    {
        cubePrefab = Resources.Load<GameObject>("Effects/xin");
        endCube = transform.Find("EndCubeshade/EndCube");
        multiple1 = transform.Find("Num1").gameObject;
        multiple2 = transform.Find("Num2").gameObject;
        hpText1 = transform.Find("hpText1").gameObject;
        hpText2 = transform.Find("hpText2").gameObject;
        hpText3 = transform.Find("hpText3").gameObject;
        scaleEnd = new Vector3(6.3f,60,60);
    }
    //重置血量
    public void SetHp(float hp)
    {
        BarText((int)hp);
        isOver = false;
        this.hp = hp;
        maxHp = hp;
        tubeHp = 1;
        scaleEnd.x = 6.3f;
        endCube.localScale= scaleEnd;
        BarMultiple(UIManager.Instance.turretPanel.cakeGrade);
        max_HP = UIManager.Instance.turretPanel.cakeGrade*100;
        UIManager.Instance.CurretHp(max_HP);
    }
    public void AddHp()
    {
        max_HP += 100;
        UIManager.Instance.CurretHp(max_HP);
    }

    public void BarMultiple(int num)
    {
        if(num >= 10)
        {
            multiple1.GetComponent<MeshFilter>().mesh = meshes[num / 10 % 10];
            multiple2.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
        }
        else
        {
            multiple1.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
            multiple2.GetComponent<MeshFilter>().mesh = null;
        }
    }
    public void BarText(int num)
    {
        if (num >= 100)
        {
            hpText1.GetComponent<MeshFilter>().mesh = meshes[1];
            hpText2.GetComponent<MeshFilter>().mesh = meshes[0];
            hpText3.GetComponent<MeshFilter>().mesh = meshes[0];
        }
        else if(num >= 10 && num < 100)
        {
            hpText1.GetComponent<MeshFilter>().mesh = null;
            hpText2.GetComponent<MeshFilter>().mesh = meshes[num / 10 % 10];
            hpText3.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
        }
        else if(num < 10 && num >= 0)
        {
            hpText1.GetComponent<MeshFilter>().mesh = null;
            hpText2.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
            hpText3.GetComponent<MeshFilter>().mesh = null;
        }
        else
        {
            hpText1.GetComponent<MeshFilter>().mesh = null;
            hpText2.GetComponent<MeshFilter>().mesh = meshes[0];
            hpText3.GetComponent<MeshFilter>().mesh = null;
        }
    }

    public bool PerfectPass()
    {
        int index = UIManager.Instance.turretPanel.cakeGrade;
        return (index*hp >= index*maxHp);
    }
    private void OnBruise(float harm)
    {
        if(hp >= 0)
        {
            hp -= harm;
            BarText((int)hp);
            max_HP -= harm;
            UIManager.Instance.CurretHp(max_HP);
            if (hp <= 0 && !isOver)
            {
                NextGame();
            }
            else
            {
                AudioManager.Instance.PlayTouch("cakehurt_1");
                scaleEnd.x = (hp / maxHp) * 6.3f;
                if (scaleEnd.x < 0) scaleEnd.x = 0;
                endCube.DOScaleX(scaleEnd.x, 0.3f);
            }
        }
    }
    private void AddBruise(float harm)
    {
        hp += harm;
        if (tubeHp == 1 && hp > 100)
        {
            hp = 100;
        }
        else if(hp > 100)
        {
            hp = hp - 100;
            tubeHp -= 1;
            BarMultiple(UIManager.Instance.turretPanel.cakeGrade - tubeHp);
        }
        max_HP += harm;
        UIManager.Instance.CurretHp(max_HP);
        BarText((int)hp);
    }

    private void NextGame()
    {
        if (UIManager.Instance.turretPanel.cakeGrade - tubeHp > 0)
        {
            BarMultiple((int)UIManager.Instance.turretPanel.cakeGrade - tubeHp);
            tubeHp += 1;
            this.hp += 100;
            if(hp > 0)
            {
                BarText((int)hp);
                scaleEnd.x = (hp / maxHp) * 6.3f;
                if (scaleEnd.x < 0) scaleEnd.x = 0;
                endCube.transform.localScale = scaleEnd;
            }
            else
            {
                NextGame();
            }
        }
        else
        {
            AudioManager.Instance.PlayTouch("cakedead_1");
            BarMultiple(1);
            TurretDrag.Instance.CakePixel(transform.localPosition);
            isOver = true;
            StartCoroutine(DelyLevel());
        }
       
    }

    private IEnumerator DelyLevel()
    {
        yield return new WaitForSeconds(1);
        WaterAnim(true);
        if(GameManager.Instance.modeSelection == "hook")
        {
            KeelAnimation.Instance.FailArmature();
            yield return new WaitForSeconds(2.5f);
            UIManager.Instance.isTime = true;
            if (PlayerPrefs.GetString("Guide10") == "")
            {
                UIManager.Instance.guidePanel.OpenGuide(10, true);
            }
            else
            {
                UIManager.Instance.failurePanel.gameObject.SetActive(true);
            }
        }
        else if(GameManager.Instance.modeSelection == "time")
        {
            UIManager.Instance.settlePanel.OpenSettle();
        }
        yield return new WaitForSeconds(0.2f);
        CreateModel.Instance.GlobalDestruction();
    }
    private IEnumerator CreateEffects(Vector3 pos,float dely)
    {
        yield return new WaitForSeconds(dely);
        float ran = Random.Range(-1f,1f);
        pos.x += ran;
        var go = ObjectPool.Instance.CreateObject(cubePrefab.name,cubePrefab);
        go.transform.localPosition = pos;
        go.GetComponent<EndCubeEff>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            other.transform.DOScale(Vector3.zero, 1f);
            StartCoroutine(ObjectNarrow(other.GetComponent<EnemyControl>(),null));
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            other.gameObject.SetActive(false);
            CreateModel.Instance.enemyBullets.Remove(other.transform);
            StartCoroutine(ObjectNarrow(null,other.GetComponent<EnemyBullet>()));
        }
        else if (other.CompareTag("Player"))
        {
            AddBruise(other.transform.parent.GetComponent<SkillSnowman>().heartNum);
            for (int i = 0; i < 5; i++)
            {
                StartCoroutine(RestoreHp(i*0.3f));
            }
        }
    }
    private IEnumerator ObjectNarrow(EnemyControl enemy,EnemyBullet bullet)
    {
        yield return new WaitForSeconds(1f);
        if(enemy || bullet)
        {
            float sou_Max = 0; Vector3 point;
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
            OnBruise(sou_Max);
            if (!isOver)
            {
                sou_Max = Mathf.Clamp(sou_Max, 1, 10);
                for (int i = 0; i < sou_Max; i++)
                {
                    StartCoroutine(CreateEffects(point, 0.2f * i));
                }
                if (enemy != null)
                {
                    CreateModel.Instance.LevelControl(enemy.enemyData.soul, false);
                    CreateModel.Instance.enemys.Remove(enemy.transform);
                    ObjectPool.Instance.CollectObject(enemy.gameObject);
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
        }
    }
    private IEnumerator RestoreHp(float dely)
    {
        yield return new WaitForSeconds(dely);
        float ran = Random.Range(-1f, 1f);
        Vector3 point = transform.position;
        point.x += ran;
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
        if(isHide)
        {
            water.gameObject.SetActive(true);
            water.DOLocalMoveY(15f, 1.5f);
        }
        else
        {
            water.DOLocalMoveY(-6,5);
            Invoke("HideWater",2);
        }
    }
    void HideWater()
    {
        water.gameObject.SetActive(false);
    }
}
