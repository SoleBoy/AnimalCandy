using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ATTAuth : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void _RequestTrackingAuthorizationWithCompletionHandler();

    [DllImport("__Internal")]
    private static extern int _GetAppTrackingAuthorizationStatus();

    private static Action<int> getAuthorizationStatusAction;

    /// <summary>
    /// 请求ATT授权窗口
    /// </summary>
    /// <param name="getResult"></param>
    public static void RequestTrackingAuthorizationWithCompletionHandler(Action<int> getResult)
    {
        //-1:"ios版本低于14"
        //0: "ATT 授权状态待定";
        //1: "ATT 授权状态受限";
        //2: "ATT 已拒绝";
        //3: "ATT 已授权";
        Debug.Log("RequestTrackingAuthorizationWithCompletionHandler");
        getAuthorizationStatusAction = getResult;
        _RequestTrackingAuthorizationWithCompletionHandler();
    }

    /// <summary>
    /// 获取当前ATT授权状态
    /// </summary>
    /// <returns></returns>
    public static int GetAppTrackingAuthorizationStatus()
    {
        return _GetAppTrackingAuthorizationStatus();
    }

    public void GetAuthorizationStatus(string status)
    {
        getAuthorizationStatusAction?.Invoke(int.Parse(status));
    }

}
