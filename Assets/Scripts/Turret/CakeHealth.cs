using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeHealth : MonoBehaviour
{
    private GameObject multiple1;
    private GameObject multiple2;

    private GameObject hpText1;
    private GameObject hpText2;
    private GameObject hpText3;

    private CakeController cakeCon;
    private void Awake()
    {
        multiple1 = transform.Find("Num1").gameObject;
        multiple2 = transform.Find("Num2").gameObject;
        hpText1 = transform.Find("Hp1").gameObject;
        hpText2 = transform.Find("Hp2").gameObject;
        hpText3 = transform.Find("Hp3").gameObject;
    }
    private void Start()
    {
        cakeCon = transform.parent.GetComponent<CakeController>();
    }
    public void BarMultiple(Mesh[] meshes,int num)
    {
        if (num >= 10)
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
    public void BarText(Mesh[] meshes,int num)
    {
        if (num >= 100)
        {
            hpText1.GetComponent<MeshFilter>().mesh = meshes[1];
            hpText2.GetComponent<MeshFilter>().mesh = meshes[0];
            hpText3.GetComponent<MeshFilter>().mesh = meshes[0];
        }
        else if (num >= 10 && num < 100)
        {
            hpText1.GetComponent<MeshFilter>().mesh = null;
            hpText2.GetComponent<MeshFilter>().mesh = meshes[num / 10 % 10];
            hpText3.GetComponent<MeshFilter>().mesh = meshes[num / 1 % 10];
        }
        else if (num < 10 && num >= 0)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet")|| other.CompareTag("Player"))
        {
            cakeCon.TriggerEnemy(other.transform);
        }
    }
}
