using UnityEngine;
using System.Collections;

public class JointFollow : MonoBehaviour
{
    public Camera foregroundCamera;

    public int playerIndex = 0;

    public Transform leftHandOverlay;
    public Transform rightHandOverlay;

    public float smoothFactor = 10f;
    private KinectManager manager;

    void Update()
    {
        if (manager == null)
        {
            manager = KinectManager.Instance;
        }

        if (manager && manager.IsInitialized() && foregroundCamera)
        {
            Rect backgroundRect = foregroundCamera.pixelRect;
            PortraitBackground portraitBack = PortraitBackground.Instance;

            if (portraitBack && portraitBack.enabled)
            {
                backgroundRect = portraitBack.GetBackgroundRect();
            }

            if (manager.IsUserDetected())
            {
                long userId = manager.GetUserIdByIndex(playerIndex);

                OverlayJoint(userId, (int)KinectInterop.JointType.HandLeft, leftHandOverlay, backgroundRect);
                OverlayJoint(userId, (int)KinectInterop.JointType.HandRight, rightHandOverlay, backgroundRect);
            }

        }
    }

    private void OverlayJoint(long userId, int jointIndex, Transform overlayObj, Rect backgroundRect)
    {
        if (manager.IsJointTracked(userId, jointIndex))
        {
            Vector3 posJoint = manager.GetJointKinectPosition(userId, jointIndex);
            if (posJoint != Vector3.zero)
            {
                Vector2 posDepth = manager.MapSpacePointToDepthCoords(posJoint);
                ushort depthValue = manager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

                if (depthValue > 0)
                {
                    Vector2 posColor = manager.MapDepthPointToColorCoords(posDepth, depthValue);

                    float xNorm = (float)posColor.x / manager.GetColorImageWidth();
                    float yNorm = 1.0f - (float)posColor.y / manager.GetColorImageHeight();

                    if (overlayObj && foregroundCamera)
                    {
                        float distanceToCamera = overlayObj.position.z - foregroundCamera.transform.position.z;
                        posJoint = foregroundCamera.ViewportToWorldPoint(new Vector3(xNorm, yNorm, distanceToCamera));

                        //overlayObj.position = posJoint;
                        overlayObj.position = Vector3.Lerp(overlayObj.position, posJoint, Time.deltaTime * smoothFactor);
                    }
                }
            }
        }
    }

}
