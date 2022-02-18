using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private GameObject parentGame;

    private DragonBones.UnityArmatureComponent pause_mode;

    private int indexRoude;
    public void Init()
    {
        parentGame = transform.Find("Parent").gameObject;
        pause_mode = transform.Find("pause").GetComponent<DragonBones.UnityArmatureComponent>();
    }

    public void OpenPause()
    {
        parentGame.SetActive(false);
        gameObject.SetActive(true);
        pause_mode.animation.Play("pause_1", 1);
        pause_mode.AddEventListener(DragonBones.EventObject.COMPLETE, OpenWin);
    }
    public void ClosePause(int index)
    {
        indexRoude = index;
        parentGame.SetActive(false);
        pause_mode.animation.Play("pause_3", 1);
        pause_mode.AddEventListener(DragonBones.EventObject.COMPLETE, CloseWin);
    }

    private void OpenWin(string type, DragonBones.EventObject eventObject)
    {
        parentGame.SetActive(true);
        pause_mode.animation.Play("pause_2", 1);
        pause_mode.RemoveEventListener("complete", OpenWin);
    }

    private void CloseWin(string type, DragonBones.EventObject eventObject)
    {
        pause_mode.RemoveEventListener("complete", CloseWin);
        if (indexRoude == 1)//退出
        {
            UIManager.Instance.settlePanel.OpenSettle(false);
        }
        else if (indexRoude == 2)//重玩
        {
            UIManager.Instance.routeMapPanel.SelectLevel(CreateModel.Instance.level);
        }
        else if (indexRoude == 3)//继续
        {
            UIManager.Instance.isTime = false;
        }
        gameObject.SetActive(false);
    }
}
