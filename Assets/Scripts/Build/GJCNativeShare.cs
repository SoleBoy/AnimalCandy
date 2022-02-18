using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IPHONE && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class GJCNativeShare : MonoBehaviour
{
#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void _GJC_NativeShare(string text, string encodedMedia);
#endif

    public delegate void OnShareSuccess(string platform);
    public delegate void OnShareCancel(string platform);

    public OnShareSuccess onShareSuccess = null;
    public OnShareCancel onShareCancel = null;

    private static GJCNativeShare _instance = null;
    public static GJCNativeShare Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(GJCNativeShare)) as GJCNativeShare;
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<GJCNativeShare>();
                    _instance.gameObject.name = _instance.GetType().FullName;
                }
            }
            return _instance;
        }
    }

    public void NativeShare(string text, Texture2D texture = null)
    {
        Debug.Log("NativeShare");
#if UNITY_IPHONE && !UNITY_EDITOR
			if(texture != null) {
				Debug.Log("NativeShare: Texture");
				byte[] val = texture.EncodeToPNG();
				string bytesString = System.Convert.ToBase64String (val);
				_GJC_NativeShare(text, bytesString);
			} else {
				Debug.Log("NativeShare: No Texture");
				_GJC_NativeShare(text, "");
			}
#endif
    }
    private void OnNativeShareSuccess(string result)
    {
        // Debug.Log("success: " + result);
        if (onShareSuccess != null)
        {
            onShareSuccess(result);
            Debug.Log("分享成功");
        }
    }
    private void OnNativeShareCancel(string result)
    {
        // Debug.Log("cancel: " + result);
        if (onShareCancel != null)
        {
            onShareCancel(result);
            Debug.Log("分享失败");
        }
    }


    public void ShareWithNative(GameObject gameName, GameObject go)
    {
        StartCoroutine(TakeScreenshot(gameName, go));
    }

    private IEnumerator TakeScreenshot(GameObject gameName, GameObject go)
    {
        yield return new WaitForEndOfFrame();
        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        NativeShare("Animal Candy", tex);
        Destroy(tex);
        gameName.SetActive(false);
        go.SetActive(false);
    }
}