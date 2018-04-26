using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 显示Kinect图像
/// </summary>
public class ShowKinectTexture : MonoBehaviour
{
    public Camera foregroundCamera;
    public RawImage backgroundImage;
    public bool debugTexture = false;
    public float texScale = 1;
    private KinectManager manager;
    private Texture2D foregroundTex;
    //矩形的前景纹理(像素)
    private Rect foregroundGuiRect;
    private Rect foregroundImgRect;
    private int depthImageWidth;
    private int depthImageHeight;

    void Start()
    {
        manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {
            KinectInterop.SensorData sensorData = manager.GetSensorData();

            if (sensorData != null && sensorData.sensorInterface != null && foregroundCamera != null)
            {
                // get depth image size
                depthImageWidth = sensorData.depthImageWidth;
                depthImageHeight = sensorData.depthImageHeight;

                // calculate the foreground rectangles
                Rect cameraRect = foregroundCamera.pixelRect;
                // print(cameraRect);
                float rectHeight = cameraRect.height;
                float rectWidth = cameraRect.width;

                if (rectWidth > rectHeight)
                    rectWidth = rectHeight * depthImageWidth / depthImageHeight;
                else
                    rectHeight = rectWidth * depthImageHeight / depthImageWidth;

                rectWidth *= texScale;
                rectHeight *= texScale;

                float foregroundOfsX = (cameraRect.width - rectWidth) / 2;
                float foregroundOfsY = (cameraRect.height - rectHeight) / 2;
                foregroundImgRect = new Rect(foregroundOfsX, foregroundOfsY, rectWidth, rectHeight);
                foregroundGuiRect = new Rect(foregroundOfsX, cameraRect.height - foregroundOfsY, rectWidth, -rectHeight);
            }
        }

    }

    void Update()
    {
        if (manager == null)
        {
            manager = KinectManager.Instance;
        }

        if (manager && manager.IsInitialized())//如果初始化成功
        {
            if (manager.IsUserDetected())
            {
                backgroundImage.color = Color.white;
                if (backgroundImage)
                {
                    foregroundTex = manager.GetUsersLblTex();
                    backgroundImage.texture = manager.GetUsersLblTex();//获取深度数据流
                }
            }
        }
    }

    void OnGUI()
    {
        if (debugTexture)
        {
            if (foregroundTex)
            {
                GUI.DrawTexture(foregroundGuiRect, foregroundTex);
            }
        }
    }
}
