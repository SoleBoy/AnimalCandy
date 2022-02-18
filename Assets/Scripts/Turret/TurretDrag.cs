using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretDrag : MonoBehaviour
{
    private static TurretDrag instance;
    public static TurretDrag Instance { get => instance; }
    private Transform _trans;
    private Transform otherTrans;
    private Vector3 _startPoint;
    private Vector3 _vec3TargetScreenSpace;
    private Vector3 _vec3MouseScreenSpace;
    private Vector3 _vec3TargetWorldSpace;
    private Vector3 _vec3Offset;
    //private Transform tempObject;
    private Transform pixel;
    private GameObject fitEffects;
    private GameObject deathRem;
    private TurretControl tempTurret;
    private TurretControl triggerTurret;
    public GameObject fitTips;
    public Transform mixEff;
    public ParticleSystem[] holeBlue;
    public Color[] turrets;

    private bool isHole;
    private bool isDrag;
    private bool isClick;
    private bool isGuideinfo;
    private float dragTime;
    private int recycleIndex = -10;
    private void Awake()
    {
        instance = this;
        deathRem = Resources.Load<GameObject>("Effects/DeathRem");
        fitEffects = Resources.Load<GameObject>("Effects/Colour");
        pixel = Resources.Load<Transform>("Effects/PixelBlock");
        isGuideinfo = PlayerPrefs.GetString("guideinfo3") == "";
        if (GameManager.Instance.modeSelection == "roude")
        {
            pixel.gameObject.layer = 10;
        }
        else
        {
            pixel.gameObject.layer = 0;
        }
    }
    private void Update()
    {
        if (UIManager.Instance.isTime || IsPointerOverGameObject(Input.mousePosition)) return;
        if(Input.GetMouseButtonDown(0))
        {
            if (_trans == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layer = LayerMask.GetMask("Turret");
                if (Physics.Raycast(ray, out hit, 1000, layer))
                {
                    if (hit.transform.CompareTag("Turret"))
                    {
                        tempTurret = hit.transform.GetComponent<TurretControl>();
                        if (tempTurret.isMerge)
                        {
                            AudioManager.Instance.PlayTouch("touchup_1");
                            _trans = hit.transform;
                            otherTrans = _trans.parent;
                            _startPoint = _trans.position;
                            tempTurret.HideFire();
                            dragTime = 0;
                            isDrag = true;
                            isClick = true;
                        }
                    }
                }
            }
            else if(_trans)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layer = LayerMask.GetMask("Fort");
                if (Physics.Raycast(ray, out hit, 1000, layer))
                {
                    tempTurret.otherObj = hit.transform.parent;
                    PlaceTurret();
                    if (isGuideinfo && PlayerPrefs.GetString("guideinfo2") != "")
                    {
                        isGuideinfo = false;
                        UIManager.Instance.GuideInfo("");
                        CreateModel.Instance.sendTroops = false;
                        PlayerPrefs.SetString("guideinfo3", "index");
                    }
                }
                else
                {
                    PlaceTurret();
                }
                isClick = false;
            }
        }
        if(isClick)
        {
            if (isDrag)
            {
                dragTime += Time.deltaTime;
                if (dragTime <= 0.2f)
                {
                    if(Input.GetMouseButtonUp(0))
                    {
                        isClick = false;
                        if (isGuideinfo)
                        {
                            CreateModel.Instance.sendTroops = true;
                            UIManager.Instance.GuideInfo(ExcelTool.lang["guideinfo3"]);
                            PlayerPrefs.SetString("guideinfo2", "index");
                        }
                    }
                }
                else
                {
                    isDrag = false;
                }
            }
            else
            {
                if(_trans != null)
                {
                    DragTurret();
                }
            }
        }
    }
    private void DragTurret()
    {
        isHole = false;
        if (tempTurret.isTrigger)
        {
            fitTips.transform.position = tempTurret.otherObj.position;
            fitTips.gameObject.SetActive(true);
            //tempObject = tempTurret.otherObj;
            if (tempTurret.otherObj.name != "Rubbish" && tempTurret.otherObj != otherTrans && tempTurret.otherObj.childCount >= 3)
            {
                triggerTurret = tempTurret.otherObj.GetChild(2).GetComponent<TurretControl>();
                if ((tempTurret.state == triggerTurret.state && tempTurret.grade == triggerTurret.grade))
                {
                    isHole = true;
                    if (holeBlue[0].isPlaying)
                    {
                        holeBlue[0].transform.position = _trans.position;
                        holeBlue[1].transform.position = tempTurret.otherObj.GetChild(2).position;
                    }
                }
            }
        }
        else
        {
            fitTips.gameObject.SetActive(false);
        }
        SetHoleBlue(isHole);
        if (Input.GetMouseButtonUp(0))
        {
            PlaceTurret();
        }
        else
        {
            _vec3TargetScreenSpace = Camera.main.WorldToScreenPoint(_trans.position);
            _vec3MouseScreenSpace = Input.mousePosition;
            _vec3MouseScreenSpace.z = _vec3TargetScreenSpace.z;
            _vec3TargetWorldSpace = Camera.main.ScreenToWorldPoint(_vec3MouseScreenSpace);
            _trans.position = new Vector3(_vec3TargetWorldSpace.x, 2.3f, _vec3TargetWorldSpace.z + 0.6f);
        }
    }
    //放置
    private void PlaceTurret()
    {
        tempTurret.isMerge = true;
        CreateModel.Instance.HideCueCone();
        fitTips.gameObject.SetActive(false);
        if (tempTurret.otherObj != otherTrans)
        {
            if (tempTurret.otherObj.name == "Rubbish")
            {
                if (CreateModel.Instance.LastTurret())
                {
                    if (tempTurret.grade - CreateModel.Instance.turretGrade >= 5)
                    {
                        _trans.SetParent(tempTurret.otherObj);
                        _trans.localPosition = CreateModel.Instance.turretPoint;
                        UIManager.Instance.ConfirmPanel();
                    }
                    else
                    {
                        SetTurretDes(true);
                    }
                    return;
                }
                else
                {
                    tempTurret.otherObj = otherTrans;
                    GameManager.Instance.CloneTip(ExcelTool.lang["tip9"]);
                }
            }
            else
            {
                if (tempTurret.otherObj.childCount >= 3)
                {
                    tempTurret.JudeGrade(otherTrans);
                }
            }
        }
        tempTurret.OpenRingTip();
        tempTurret.TurretLaunch(tempTurret.otherObj);
        UIManager.Instance.IsCreateTureet();
        CreateModel.Instance.RankJudgment();
        SetHoleBlue(false);
        _trans = null;
    }
    //合体提示
    private void SetHoleBlue(bool isHide)
    {
        if (holeBlue[0].gameObject.activeInHierarchy != isHide)
        {
            for (int i = 0; i < holeBlue.Length; i++)
            {
                holeBlue[i].gameObject.SetActive(isHide);
                if (isHide)
                {
                    holeBlue[i].Play();
                }
                else
                {
                    holeBlue[i].Stop();
                }
            }
        }
    }

    public void HideEffect()
    {
        fitTips.gameObject.SetActive(false);
        SetHoleBlue(false);
    }
    //销毁炮塔
    public void SetTurretDes(bool isDes)
    {
        if(isDes)
        {
            CreateModel.Instance.turrets.Remove(_trans);
            AudioManager.Instance.PlayTouch("shooter_dead");
            UIManager.Instance.ColeGold(tempTurret.otherObj.position, (5 + 3 * (tempTurret.grade - 1)) * 0.2f);
            tempTurret.PlayDeath();
            _trans.SetParent(tempTurret.otherObj);
            tempTurret.otherObj.GetComponent<Animator>().Play("rubbishbin", 0, 0);
            if (otherTrans.name.Contains("fire"))
            {
                CreateModel.Instance.ShowTipCone(otherTrans.GetSiblingIndex());
            }
            for (int i = 0; i < Random.Range(4, 8); i++)
            {
                StartCoroutine(DestroyEffects(_trans.position, i * 0.3f));
            }
            if (recycleIndex == (int)tempTurret.state)
            {
                recycleIndex = -10;
                UIManager.Instance.statePanel.SetActive(true);
                UIManager.Instance.isTime = true;
            }
            else
            {
                recycleIndex = (int)tempTurret.state;
            }
        }
        else
        {
            AudioManager.Instance.PlayTouch("touchdown_1");
            tempTurret.TurretLaunch(otherTrans);
        }
        UIManager.Instance.IsCreateTureet();
        _trans = null;
    }
    //销毁特效
    private IEnumerator DestroyEffects(Vector3 pos,float dely)
    {
        yield return new WaitForSeconds(dely);
        float ran = Random.Range(0f, 0.5f);
        pos.x -= ran;
        pos.z += ran;
        var go = ObjectPool.Instance.CreateObject("deatheffects", deathRem);
        go.transform.localPosition = pos;
        go.GetComponent<EndCubeEff>().enabled = true;
    }
    public bool IsPointerOverGameObject(Vector2 screenPosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            //实例化点击事件
            PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            //将点击位置的屏幕坐标赋值给点击事件
            eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            //向点击处发射射线
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }
        return false;
    }
    //动物受伤
    public void EenemyPixel(GameObject enemy,int index, bool isEnemy)
    {
        Vector2 size=enemy.GetComponent<EnemyControl>().sizeScale.GetComponent<Renderer>().bounds.size;
        int ran = UnityEngine.Random.Range(1,5);
        if(isEnemy)
        {
            for (int i = 0; i < ran; i++)
            {
                StartCoroutine(CubeBoos(size, enemy.transform.position, i * 0.02f, ExcelTool.Instance.animalColor[index - 1]));
            }
        }
        else
        {
            for (int i = 0; i < ran; i++)
            {
                StartCoroutine(CubeEnemy(size, enemy.transform.position, i * 0.01f, ExcelTool.Instance.animalColor[index - 1]));
            }
        }
    }
    //boos死亡
    public void BoosPixel(Vector3 pos)
    {
        int ran = UnityEngine.Random.Range(10, 30);
        for (int i = 0; i < ran; i++)
        {
            StartCoroutine(CubeBoos(Vector2.one, pos, i * 0.01f, ExcelTool.Instance.animalColor[Random.Range(0, ExcelTool.Instance.animalColor.Length)]));
        }
    }
    //蛋糕死亡
    public void CakePixel(Vector3 pos)
    {
        int ran = UnityEngine.Random.Range(25, 45);
        for (int i = 0; i < ran; i++)
        {
            StartCoroutine(CakeCube(pos, i * 0.02f, new Color(1,0,0,0.7f)));
        }
    }
    //合成特效
    public void Composite(Vector3 pos,int index)
    {
        var mix = ObjectPool.Instance.CreateObject(mixEff.name,mixEff.gameObject);
        mix.gameObject.SetActive(true);
        mix.transform.localPosition = pos;
        mix.GetComponent<Animator>().Play("MixEff",0,0);
        AudioManager.Instance.PlaySource("fit_1", mix.GetComponent<AudioSource>());
        StartCoroutine(HidComposite(mix));
        int ran = Random.Range(5, 10);
        for (int i = 0; i < ran; i++)
        {
            StartCoroutine(CubeBoos(Vector2.one,pos,i * 0.01f+0.2f, turrets[index],0.15f));
        }
    }
    IEnumerator HidComposite(GameObject go)
    {
        yield return new WaitForSeconds(0.8f);
        ObjectPool.Instance.CollectObject(go);
    }
    //Boos受伤方块
    IEnumerator CubeBoos(Vector2 size,Vector3 pos, float day,Color color, float mult = 0.3f)
    {
        yield return new WaitForSeconds(day);
        var go = ObjectPool.Instance.CreateObject("PixelBlock", pixel.gameObject);
        go.transform.localScale = Vector3.one * mult;
        go.GetComponent<Renderer>().material.color = color;
        pos.x += Random.Range(-size.x * 0.5f, size.x * 0.5f);
        pos.y += size.y;
        go.transform.localPosition = pos;
        go.transform.Rotate(Vector3.up * UnityEngine.Random.Range(1, 30));
        go.GetComponent<PixelBlock>().SetPower(1);
    }
    //动物受伤方块
    IEnumerator CubeEnemy(Vector2 size, Vector3 pos, float day, Color color, float mult = 0.3f)
    {
        yield return new WaitForSeconds(day);
        var go = ObjectPool.Instance.CreateObject("PixelBlock", pixel.gameObject);
        go.transform.localScale = Vector3.one * mult;
        go.GetComponent<Renderer>().material.color = color;
        pos.x += Random.Range(-size.x * 0.5f, size.x * 0.5f);
        pos.y -= size.y;
        pos.z -= size.y * 0.5f;
        go.transform.localPosition = pos;
        go.transform.Rotate(Vector3.up * UnityEngine.Random.Range(1, 30));
        go.GetComponent<PixelBlock>().SetPower(1);
    }
    //蛋糕死亡
    IEnumerator CakeCube(Vector3 pos, float day, Color color, float mult = 0.3f)
    {
        yield return new WaitForSeconds(day);
        var go = ObjectPool.Instance.CreateObject("PixelBlock", pixel.gameObject);
        go.transform.localScale = Vector3.one * mult;
        go.GetComponent<Renderer>().material.color = color;
        pos.x += UnityEngine.Random.Range(-4, 5);
        pos.y += 0.5f;
        go.transform.localPosition = pos;
        go.transform.localEulerAngles=Vector3.zero;
        go.transform.Rotate(-Vector3.left * UnityEngine.Random.Range(15, 35));
        go.GetComponent<PixelBlock>().SetPower(10);

    }

    public void Meage(Vector3 point)
    {
        var colour = ObjectPool.Instance.CreateObject("fiteffects", fitEffects);
        //Vector3 point = transform.parent.localPosition;
        point.y += 0.5f;
        colour.transform.localPosition = point;
        colour.GetComponent<ParticleSystem>().Play();
        StartCoroutine(HideMeger(colour));
    }

    IEnumerator HideMeger(GameObject go)
    {
        yield return new WaitForSeconds(2);
        ObjectPool.Instance.CollectObject(go);
    }

}
