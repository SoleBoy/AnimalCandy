using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Egg_Bullet : MonoBehaviour
{
    private Transform eggshell;
    private Transform yolk;

    private Material material_shell;
    private Material material_yolk;

    private Vector3 scale_eggshell;
    private Vector3 scale_yolk;

    private Color color;
    private void Awake()
    {
        eggshell = transform.Find("eggshell");
        yolk = transform.Find("yolk");
        material_shell = eggshell.GetComponent<Renderer>().material;
        material_yolk = yolk.GetComponent<Renderer>().material;
        scale_eggshell = eggshell.localScale;
        scale_yolk = yolk.localScale;
        color = material_shell.color;
        color.a = 0.3f;
        material_shell.color = color;
    }
    public void OpenAnimal()
    {
        eggshell.localScale = Vector3.zero;
        yolk.localScale = Vector3.zero;
        StartCoroutine(EggAnimal());
    }

    IEnumerator EggAnimal()
    {
        material_yolk.DOFade(1, 0.1f);
        yolk.DOScale(scale_yolk,0.1f);
        yield return new WaitForSeconds(0.1f);
        yolk.DOScale(scale_yolk*1.1f, 0.2f);
        eggshell.DOScale(scale_eggshell,0.2f);
        yield return new WaitForSeconds(0.2f);
        yolk.DOScale(scale_yolk * 1.15f, 0.2f);
        eggshell.DOScale(scale_eggshell * 1.1f, 0.2f);
        material_shell.DOFade(0,0.2f);
        material_yolk.DOFade(0,0.2f);
        yield return new WaitForSeconds(0.2f);
        material_shell.color = color;
        gameObject.SetActive(false);
    }
}
