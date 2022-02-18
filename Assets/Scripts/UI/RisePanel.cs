using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RisePanel : MonoBehaviour
{
    private Button fixeBtn;
    private Button cancelBtn;
    private Button dimaBtn;
    private void Awake()
    {
        dimaBtn = transform.Find("DiamBtn").GetComponent<Button>();
        fixeBtn = transform.Find("YesBtn").GetComponent<Button>();
        cancelBtn = transform.Find("NoBtn").GetComponent<Button>();

        fixeBtn.onClick.AddListener(OpenVideo);
        cancelBtn.onClick.AddListener(ClosePanel);
        dimaBtn.onClick.AddListener(Resurrection);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        UIManager.Instance.isTime = true;
    }

    private void ClosePanel()
    {
        CreateModel.Instance.cakeCon.CancelRise();
        gameObject.SetActive(false);
    }

    private void OpenVideo()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            bool isReward = false;
            GameManager.Instance.UserChoseToWatchAd(AdsType.diamond);
            GameManager.Instance.AdmobRewardCB = delegate
            {
                UIManager.Instance.isTime = true;
            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isReward = true;
            };
            GameManager.Instance.AdmobClose = delegate
            {
                if(isReward)
                {
                    CreateModel.Instance.cakeCon.RiseHealth();
                }
                else
                {
                    CreateModel.Instance.cakeCon.CancelRise();
                }
                gameObject.SetActive(false);
                UIManager.Instance.isTime = false;
                GameManager.Instance.RequestRewardBasedVideo(AdsType.diamond);
            };
        }
        else
        {
            gameObject.SetActive(false);
            UIManager.Instance.isTime = false;
            CreateModel.Instance.cakeCon.RiseHealth();
        }
    }

    private void Resurrection()
    {
        if(UIManager.Instance.starNumber >= 1)
        {
            UIManager.Instance.SetStar(-1);
            gameObject.SetActive(false);
            UIManager.Instance.isTime = false;
            CreateModel.Instance.cakeCon.RiseHealth();
        }
        else
        {
            UIManager.Instance.diamondPanel.OpenPanel();
        }
    }
}
