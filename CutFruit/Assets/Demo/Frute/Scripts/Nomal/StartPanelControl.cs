using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 开始面板控制
/// </summary>
public class StartPanelControl : MonoBehaviour
{
    public RectTransform canvas;
    public CutLineControl leftLine;
    public CutLineControl rightLine;
    //三个圆盘UI
    public RectTransform c1;
    public RectTransform c2;
    public RectTransform c3;
    //三个自带重力的水果 
    public RectTransform fruit1;
    public RectTransform fruit2;
    public RectTransform fruit3;

    public GameObject fruit1Cut;
    public GameObject fruit2Cut;

	private int _zFruit = 0;
	private int _zCircle = 0;
	[SerializeField]
	public float _time = 3f;

    Rigidbody2D[] rigi;
	private RectTransform[] circle = new RectTransform[3];
    void Start()
    {
        rigi = new Rigidbody2D[3];
        rigi[0] = fruit1.GetComponent<Rigidbody2D>();
        rigi[1] = fruit2.GetComponent<Rigidbody2D>();
        rigi[2] = fruit3.GetComponent<Rigidbody2D>();

		circle = new RectTransform[3];
		circle[0] = c1.GetComponent<RectTransform>();
		circle[1] = c2.GetComponent<RectTransform>();
		circle[2] = c3.GetComponent<RectTransform>();
    }

    bool isCut;

    KinectManager _manager;

	private void RotatePic()
	{
		_zFruit -= 10;
		_zCircle += 10;
		for(int i = 0; i < rigi.Length; i++)
		{
			if(rigi[i] != null)
			{
				if ( i == 2 ) 
				{
					rigi[i].transform.Rotate (new Vector3(0,0,0),_time);
				}
				else
				{
					rigi[i].transform.Rotate (new Vector3(0,0,_zFruit),_time);
				}

			}
			if(circle[i] != null)
			{
				circle[i].transform.Rotate (new Vector3(0,0,_zCircle),_time);
			}
		}
	}

    void Update()
    {
        bool isOverBtn1 = false, isOverBtn2 = false, isOverBtn3 = false;

        if (_manager == null)
        {
            _manager = KinectManager.Instance;
        }

        if (_manager && _manager.IsInitialized())//如果初始化成功
        {
            #region 检测切割，更新划线
            //先检查人物是否被检测
            if (_manager.IsUserDetected())
            {
                long userId = _manager.GetPrimaryUserID();
                //获取左右手手掌的关节视口坐标
                Vector3 leftJointPos = KinectUtils.GetViewportToScreenPoint(KinectInterop.JointType.HandLeft);
                Vector3 rightJointPos = KinectUtils.GetViewportToScreenPoint(KinectInterop.JointType.HandRight);
                //将左右手坐标转换为ugui坐标
                Vector2 leftHandScreenpos;
                bool isLeftHandOver = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, leftJointPos, Camera.main, out leftHandScreenpos);
                Vector2 rightHandScreenpos;
                bool isRightHandOver = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, rightJointPos, Camera.main, out rightHandScreenpos);

                //左手是否悬浮在区域上
                bool l1 = RectTransformUtility.RectangleContainsScreenPoint(fruit1, leftJointPos, Camera.main);
                bool l2 = RectTransformUtility.RectangleContainsScreenPoint(fruit2, leftJointPos, Camera.main);
                bool l3 = RectTransformUtility.RectangleContainsScreenPoint(fruit3, leftJointPos, Camera.main);
                //右手是否悬浮在区域上
                bool r1 = RectTransformUtility.RectangleContainsScreenPoint(fruit1, rightJointPos, Camera.main);
                bool r2 = RectTransformUtility.RectangleContainsScreenPoint(fruit2, rightJointPos, Camera.main);
                bool r3 = RectTransformUtility.RectangleContainsScreenPoint(fruit3, rightJointPos, Camera.main);

                isOverBtn1 = l1 || r1;
                isOverBtn2 = l2 || r2;
                isOverBtn3 = l3 || r3;

                //更新显示线位置
                leftLine.UpdateCutLine(leftHandScreenpos);
                rightLine.UpdateCutLine(rightHandScreenpos);
            }
            else
            {
                leftLine.UpdateCutLine(new Vector2(-1000, 1000));
                rightLine.UpdateCutLine(new Vector2(1000, 1000));
            }
            #endregion
            //检测是否选中按钮
            ClickBtnEvent(isOverBtn1, isOverBtn2, isOverBtn3);
			RotatePic();
        }
    }

    void ClickBtnEvent(bool o1, bool o2, bool o3)
    {
        bool isover = o1 || o2 || o3;
        if (isover && !isCut)
        {
            c1.gameObject.SetActive(false);
            c2.gameObject.SetActive(false);
            c3.gameObject.SetActive(false);
            if (o1)
            {
                if (fruit1.GetComponent<Image>().enabled)
                {
                    GameObject go = Instantiate(fruit1Cut, fruit1.position, fruit1.rotation) as GameObject;
                    go.transform.SetParent(fruit1);
                    go.transform.localScale = Vector3.one;
                    fruit1.GetComponent<Image>().enabled = false;
                    GameController._instance.currentIndex = 1;
                    Destroy(go, 2);
                }
            }
            else if (o2)
            {
                if (fruit2.GetComponent<Image>().enabled)
                {
                    GameObject go = Instantiate(fruit2Cut, fruit2.position, fruit2.rotation) as GameObject;
                    go.transform.SetParent(fruit2);
                    go.transform.localScale = Vector3.one;
                    fruit2.GetComponent<Image>().enabled = false;
                    GameController._instance.currentIndex = 2;
                    Destroy(go, 2);
                }
            }
            else if (o3)//切到雷
            {
                GameController._instance.currentIndex = 3;
                rigi[2].AddForce(new Vector2(0, 1200));
                //GameController._instance.Exit();
            }
            rigi[0].gravityScale = 10;
            rigi[1].gravityScale = 10;
            rigi[2].gravityScale = 10;
        }
        isCut = isover;
    }

    public void InitStartPanel(RectTransform canvas)
    {
        this.canvas = canvas;
    }
}
