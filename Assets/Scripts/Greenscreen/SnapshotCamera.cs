using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SnapshotCamera : MonoBehaviour
{
    private Camera snapCam;

    public int resWidth;// = 256;
    public int resHeight;// = 256;

    private void Awake()
    {
        snapCam = GetComponent<Camera>();
        SetResolution(resWidth, resHeight);
    }

    public void SetResolution(int width, int height)
    {
        resWidth = width;
        resHeight = height;

        if (snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
        }
        else
        {
            snapCam.targetTexture.width = resWidth;
            snapCam.targetTexture.height = resHeight;
        }
    }

    public void TakeSnapshot()
    {
        Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGBA32, false);
        snapCam.Render();
        RenderTexture.active = snapCam.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        byte[] bytes = snapshot.EncodeToPNG();
        string filename = SnapshotName();
        File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Snapshot saved to {0}", filename));
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private string SnapshotName()
    {
        string basePath;
#if UNITY_EDITOR
        basePath = Application.dataPath;
#else
        basePath = Application.persistentDataPath;
#endif
        return string.Format("{0}/Snapshots/snap_{1}.png", Application.dataPath, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
    }
}