using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能水世界
/// </summary>
public class SkillWater : MonoBehaviour
{
    Transform wall;
    Transform water_floor;
    Transform water_word;
    Transform ripple_prefab;
    Transform spray_prefab;

    Vector3 starAnjle;
    Vector3 spray_scale;
    float lastTime;
    bool isRipple;
    AudioSource source;

    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        wall = transform.Find("wall");
        water_floor = transform.Find("water_floor");
        water_word = water_floor.Find("water");
        ripple_prefab = water_floor.Find("water/water_ripple");
        spray_prefab = water_floor.Find("water/spray");
        starAnjle = water_floor.localEulerAngles;
        spray_scale = spray_prefab.localScale;
       
    }
    public void SetInit(SkillItem item,float hurt)
    {
        starAnjle.x = 0;
        water_floor.localEulerAngles = starAnjle;
        wall.localPosition = new Vector3(0, 50, 0);
        water_floor.localPosition = new Vector3(0, 60, 88);
        water_word.GetComponent<SkillData>().SetInit(item, hurt);
        StartCoroutine(WaterAnim());
    }
    void Update()
    {
        if (isRipple)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 0.5f)
            {
                lastTime = 0;
                var go = Instantiate(ripple_prefab.gameObject);//ObjectPool.Instance.CreateObject(ripple_prefab.name, ripple_prefab.gameObject);
                go.gameObject.SetActive(true);
                go.transform.SetParent(water_word, true);
                go.transform.localScale = ripple_prefab.localScale;
                go.transform.localPosition = new Vector3(0, 0.5f, 0.5f);
                go.transform.DOLocalMoveZ(-0.5f, 3);
                StartCoroutine(WaterRipple(go));
                for (int i = 0; i < 30; i++)
                {
                    var spray = Instantiate(spray_prefab);//ObjectPool.Instance.CreateObject(spray_prefab.name, spray_prefab.gameObject);
                    spray.gameObject.SetActive(true);
                    spray.transform.SetParent(water_word, true);
                    spray.transform.localScale = spray_scale*Random.Range(1f,2f);
                    spray.transform.rotation = spray_prefab.rotation;
                    spray.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, -0.5f);
                    spray.transform.GetComponent<PixelBlock>().SetPower(Random.Range(5, 10));
                }

            }
        }
    }
    IEnumerator WaterAnim()
    {
        wall.DOLocalMoveY(0,0.2f);
        water_floor.DOLocalMoveY(2f,0.5f);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySource("skill_3_1", source);
        isRipple = true;
        lastTime = 1;
        water_floor.DOLocalMoveZ(3, 3);
        yield return new WaitForSeconds(3);
        AudioManager.Instance.PlaySource("skill_3_2", source);
        starAnjle.x = 15;
        water_floor.DOLocalRotate(starAnjle, 2);
        water_floor.DOLocalMoveY(0.5f, 2);
        isRipple = false;
        yield return new WaitForSeconds(2);
        wall.DOLocalMoveY(-30, 2);
        yield return new WaitForSeconds(2);
        GameObject.Destroy(gameObject);
        //ObjectPool.Instance.Clear(ripple_prefab.name);
        //ObjectPool.Instance.Clear(spray_prefab.name);
        //ObjectPool.Instance.CollectObject(gameObject);
    }
    WaitForSeconds ripple = new WaitForSeconds(3);
    IEnumerator WaterRipple(GameObject game)
    {
        yield return ripple;
        if(game)
            game.SetActive(false);
    }

}
