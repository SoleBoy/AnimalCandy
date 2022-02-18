using System.Collections;
using System.IO;
using UnityEngine;


public class GJCNativeShare : MonoBehaviour
{
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
    private bool isProcessing = false;
    private bool isFocus = false;
    public void ShareWithNative(GameObject gameName, GameObject go)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(Share());
        }
        StartCoroutine(DelyHide(gameName, go));
        //    if (!isProcessing)
        //{
        //    StartCoroutine(TakeScreenshotAndroid(gameName, go));
        //}
    }

    //private IEnumerator TakeScreenshot(GameObject gameName,GameObject go)
    //{
    //    yield return new WaitForEndOfFrame();
    //    var width = Screen.width;
    //    var height = Screen.height;
    //    var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
    //    // Read screen contents into the texture
    //    tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    //    tex.Apply();
    //    NativeShare("Animal Candy", tex);
    //    Destroy(tex);
    //    gameName.SetActive(false);
    //    go.SetActive(false);
    //}
    private IEnumerator TakeScreenshotAndroid(GameObject gameName, GameObject go)
    {
        isProcessing = true;
        yield return new WaitForEndOfFrame();
        int width = Screen.width;

        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);

        tex.Compress(false);

        yield return new WaitForSeconds(0.1f);
        gameName.SetActive(false);
        go.SetActive(false);
        byte[] bytes = new AndroidJavaObject("android.util.Base64").CallStatic<byte[]>("decode", System.Convert.ToBase64String(tex.EncodeToPNG()), 0);
        AndroidJavaObject bitmap = new AndroidJavaObject("android.graphics.BitmapFactory").CallStatic<AndroidJavaObject>("decodeByteArray", bytes, 0, bytes.Length);
        AndroidJavaObject compress = new AndroidJavaClass("android.graphics.Bitmap$CompressFormat").GetStatic<AndroidJavaObject>("PNG");
        bitmap.Call<bool>("compress", compress, 100, new AndroidJavaObject("java.io.ByteArrayOutputStream"));
        string path = new AndroidJavaClass("android.provider.MediaStore$Images$Media").CallStatic<string>("insertImage", currentActivitytest.Call<AndroidJavaObject>("getContentResolver"), bitmap, tex.name, "");
        AndroidJavaObject uri = new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse", path);
        AndroidJavaObject sharingIntent = new AndroidJavaObject("android.content.Intent");
        sharingIntent.Call<AndroidJavaObject>("setAction", "android.intent.action.SEND");
        sharingIntent.Call<AndroidJavaObject>("setType", "image/png");
        sharingIntent.Call<AndroidJavaObject>("putExtra", "android.intent.extra.STREAM", uri);
        AndroidJavaObject createChooser = sharingIntent.CallStatic<AndroidJavaObject>("createChooser", sharingIntent, "Share your new score");
        currentActivitytest.Call("startActivity", createChooser);
        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
        Destroy(tex);
    }
    private static AndroidJavaObject currentActivitytest
    {
        get
        {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

    private IEnumerator DelyHide(GameObject gameName, GameObject go)
    {
        yield return new WaitForSeconds(0.5f);
        gameName.SetActive(false);
        go.SetActive(false);
    }
    private IEnumerator Share()
    {
        yield return new WaitForEndOfFrame();
        string destination = Application.persistentDataPath + "/Texture.png";
        if (File.Exists(destination))
        {
            File.Delete(destination);
        }
        int width = Screen.width;

        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);

        byte[] imagebytes = tex.EncodeToPNG();//转化为png图

        tex.Compress(false);//对屏幕缓存进行压缩
        File.WriteAllBytes(destination, imagebytes);//存储png图
        if (File.Exists(destination))
        {
            //FileStream file = new FileStream("file:///"+destination, FileMode.Open);
            //BinaryFormatter bf = new BinaryFormatter();
            //byte[] b = file.;

            AndroidJavaClass UnityPlayer;
            AndroidJavaClass Intent;
            AndroidJavaClass Uri;
            AndroidJavaClass Environment;

            //实例化AndroidJavaClass变量
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            Intent = new AndroidJavaClass("android.content.Intent");
            Uri = new AndroidJavaClass("android.net.Uri");
            Environment = new AndroidJavaClass("android.os.Environment");

            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject jstr_type = new AndroidJavaObject("java.lang.String", "image/png");
            AndroidJavaObject jstr_content = new AndroidJavaObject("java.lang.String", destination);

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", Intent.GetStatic<AndroidJavaObject>("ACTION_SEND"));
            intent.Call<AndroidJavaObject>("setType", jstr_type);
            intent.Call<AndroidJavaObject>("putExtra", Intent.GetStatic<AndroidJavaObject>("EXTRA_STREAM"), jstr_content);

            currentActivity.Call("startActivity", intent);
        }
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //定义AndroidJavaClass变量
        //#endif
        Destroy(tex);
    }
}