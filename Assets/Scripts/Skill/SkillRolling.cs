using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SkillRolling : MonoBehaviour
{
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }
    public void SetInit(SkillItem item, float hurt,float dely)
    {
        StartCoroutine(OpenAnimal(dely));
        GetComponent<SkillHurt>().SetInit(item, hurt);
    }

    private IEnumerator OpenAnimal(float index)
    {
        yield return new WaitForSeconds(index * 0.5f);
        AudioManager.Instance.PlaySource("flashweather_1", source);
        transform.DOLocalMoveY(4,1f);
        yield return new WaitForSeconds(1f);
        transform.DOLocalMoveZ(90, 6f);
        yield return new WaitForSeconds(6f);
        transform.DOLocalMoveY(35, 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
    }
    private void Update()
    {
        transform.Rotate(Vector3.right * 5,Space.World);
    }
}
