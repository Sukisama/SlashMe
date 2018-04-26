using UnityEngine;
using System.Collections;

public class GamePanelControl : MonoBehaviour
{
    public RectTransform canvas;
    public CutLineControl leftLine;
    public CutLineControl rightLine;
    public RectTransform gamePanel;
    public Fruit fruit;
    public int forceX = 300;
    public int forceY = 5000;
    public float creatSpeed = 1;
    public float creatTime = 2;
    public int initMaxX = 300;
    public int initY = -220;

    public GameUIControl gameUIControl;
    public int scoreNum = 0;
	private float _usedTime = 0;
	private int _allTime = 60;

    KinectManager _manager;
	private Fruit[] _currentFruitArray = new Fruit[4];
    float timer = 2;// 计时器


    void Update()
    {
        timer += Time.deltaTime * creatSpeed;
        if (timer >= creatTime)
        {
			// random time to create fruit
			int num = Random.Range(1,4);
			for(int i = 0; i < num; i++)
			{
				_currentFruitArray[i] = CreatFruit();
			}
            timer = 0;
        }
        bool isRightOverFrult = false;
        bool isLeftOverFrult = false;
        if (_manager == null)
        {
            _manager = KinectManager.Instance;
        }
		for(int i = 0; i < _currentFruitArray.Length; i++)
		{
			if(_currentFruitArray[i] != null)
			{
				RectTransform rt = _currentFruitArray[i].transform as RectTransform;
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
						isLeftOverFrult = RectTransformUtility.RectangleContainsScreenPoint(rt, leftJointPos, Camera.main);
						//右手是否悬浮在区域上
						isRightOverFrult = RectTransformUtility.RectangleContainsScreenPoint(rt, rightJointPos, Camera.main);

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
				}

				#region 切水果
				if (isLeftOverFrult || isRightOverFrult)
				{
					int t = _currentFruitArray[i].type;
					if (t == 0)//切到最终boom
					{
						//life = 0;
					}
					else//切到水果加分
					{
						scoreNum++;
						CreatLeftRightFurit(t,_currentFruitArray[i]);
					}
					Destroy(_currentFruitArray[i].gameObject);
				}
				#endregion
			}
		}
		CountTime();
		gameUIControl.SetScore(scoreNum, _allTime);
    }

	private void CountTime()
	{
		//累加每帧消耗时间
		_usedTime += Time.deltaTime;
		if (_usedTime >= 1f)//每过1秒执行一次
		{
			_allTime--;
			_usedTime = 0;
		}
	}

	//生成水果
	Fruit CreatFruit()
	{
		if (_allTime >= 1) {
			Fruit currentFruit = null;
			GameObject go = Instantiate (fruit.gameObject) as GameObject;
			go.transform.SetParent (gamePanel);
			RectTransform rt = go.transform as RectTransform;
			rt.anchoredPosition3D = Vector3.zero;
			go.transform.localScale = Vector3.one;

			//随机一个生成位置
			int initX = Random.Range (-initMaxX, initMaxX);
			rt.anchoredPosition = new Vector2 (initX, initY);
			//随机一个水果
			int type = Contant.GetOneType ();
			currentFruit = go.GetComponent<Fruit> ();
			currentFruit.SetType (type);
			//添加力量
			Rigidbody2D rigi = go.GetComponent<Rigidbody2D> ();
			//随机向上的力量
			float upForce = Random.Range (forceY * 0.8f, forceY);
			float xForce = Random.Range (forceX * 0.8f, forceX * 1.2f);
			if (initX > 0) {//生成的方向 
				xForce = -xForce;
			}
			rigi.AddForce (new Vector2 (xForce, upForce));
			return currentFruit;
		}
		else {
			return null;
		}
	}

    //切到生成的水果
	void CreatLeftRightFurit(int type, Fruit fruit)
    {
        // print(type);
		GameObject leftFruit = Instantiate(fruit.gameObject, fruit.transform.position, fruit.transform.rotation) as GameObject;
        Fruit l = leftFruit.GetComponent<Fruit>();
        l.SetType(type + 1);
		InitNewFruit(l, fruit);
        AddForce(l, -forceX);
		GameObject rightFruit = Instantiate(fruit.gameObject, fruit.transform.position, fruit.transform.rotation) as GameObject;
        Fruit r = rightFruit.GetComponent<Fruit>();
        r.SetType(type + 2);
		InitNewFruit(r, fruit);
        AddForce(r, forceX);
    }
    //生成左右两半的水果
	void InitNewFruit(Fruit f, Fruit fru)
    {
        RectTransform rt = f.transform as RectTransform;
        RectTransform curRt = fru.transform as RectTransform;
        f.transform.SetParent(gamePanel);
        rt.anchoredPosition3D = Vector3.zero;
        f.transform.localScale = new Vector3(1, 1, 1);
        rt.anchoredPosition = curRt.anchoredPosition;
    }
    //给左右两半添加力
    void AddForce(Fruit f, float force)
    {
        Rigidbody2D rigi = f.gameObject.GetComponent<Rigidbody2D>();
        rigi.AddForce(new Vector2(force * 2, force * 2));
    }
    //初始化面板属性
    public void InitGamePanel(RectTransform canvas)
    {
        this.canvas = canvas;
    }
}
