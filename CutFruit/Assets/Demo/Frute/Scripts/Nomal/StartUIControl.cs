using UnityEngine;
using System.Collections;
using DG.Tweening;
/// <summary>
/// 开始UI控制
/// </summary>
public class StartUIControl : MonoBehaviour
{
    public RectTransform top;
    public RectTransform logo1;
    public RectTransform logo2;
    public RectTransform[] btns;
    //三个圆盘UI
    public RectTransform c1;
    public RectTransform c2;
    public RectTransform c3;
    //三个自带重力的水果 
    public RectTransform fruit1;
    public RectTransform fruit2;

    public GameObject boomParticl;
    KinectManager _manager;
    void Update()
    {
        if (GameController._instance.currentIndex != 0)
        {
            return;
        }
        if (_manager == null)
        {
            _manager = KinectManager.Instance;
        }

        if (_manager && _manager.IsInitialized())//如果初始化成功
        {
            if (_manager.IsUserDetected())
            {
                if (btns[2].localScale.z == 1)
                {
                    boomParticl.SetActive(true);
                }
                c1.Rotate(Vector3.forward, -5 * Time.deltaTime);
                c2.Rotate(Vector3.forward, 5 * Time.deltaTime);
                c3.Rotate(Vector3.forward, 5 * Time.deltaTime);
                fruit1.Rotate(Vector3.forward, 10 * Time.deltaTime);
                fruit2.Rotate(Vector3.forward, -10 * Time.deltaTime);
            }
            else
            {
                boomParticl.SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        PlayAnim();
    }

    void ResetAnim()
    {
        top.anchoredPosition3D = new Vector3(0, 300, 0);
        logo1.anchoredPosition3D = new Vector3(182, 162, 0);
        logo2.anchoredPosition3D = new Vector3(-410, -100, 0);
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].localScale = Vector3.zero;
        }
    }

    void PlayAnim()//DOTWEEN插件改变动画运动
    {
        ResetAnim();
        top.DOLocalMoveY(94.5f, 1);
        logo1.DOLocalMoveY(29, 1).SetDelay(0.5f).SetEase(Ease.OutBounce);
        logo2.DOLocalMoveX(-255, 1).SetDelay(1).SetEase(Ease.OutBounce); ;
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].DOScale(1, 1).SetDelay(1).SetEase(Ease.OutBack);
        }
    }
}
