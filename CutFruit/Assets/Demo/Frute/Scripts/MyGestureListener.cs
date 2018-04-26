using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 姿势手势检测
/// </summary>
public class MyGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    public Text lable;

    void KinectGestures.GestureListenerInterface.UserDetected(long userId, int userIndex)
    {
        lable.text = "用户连接";
    }

    void KinectGestures.GestureListenerInterface.UserLost(long userId, int userIndex)
    {
        lable.text = "用户丢失";
    }

    void KinectGestures.GestureListenerInterface.GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {

    }

    bool KinectGestures.GestureListenerInterface.GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        lable.text = "用户姿势：" + gesture;
        return true;

    }

    bool KinectGestures.GestureListenerInterface.GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        return true;
    }
}
