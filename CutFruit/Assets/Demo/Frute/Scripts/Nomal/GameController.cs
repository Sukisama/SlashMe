using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 游戏控制器
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController _instance;

    public RectTransform canvas;

    public GameObject startPanel;
    public GameObject gamePanel;
	public GameObject gamePanelone;

    public Image fadePanel;
    public int currentIndex = 0;

    GameObject currentStartPanel;
    GameObject currentGamePanel;
	GameObject currentGamePanelOne;

    void Awake()
    {
        _instance = this;
        //初始化开始界面
        CreatStartPanel();
        //CreatGamePanel();
    }

    //创建开始界面
    public void CreatStartPanel()
    {
        FadePanel(1.5f);//负责面板的淡入淡出
        currentIndex = 0;
        if (currentStartPanel != null)//如果存在开始界面
        {
            Destroy(currentStartPanel);
        }

        currentStartPanel = Instantiate(startPanel) as GameObject;//实例化对象
        currentStartPanel.GetComponent<StartPanelControl>().InitStartPanel(canvas);
        currentStartPanel.transform.SetParent(transform);
		currentStartPanel.transform.localPosition = Vector3.zero;//local表示根据父节点为中心，即Vector3(0, 0, 0)
        currentStartPanel.transform.localScale = Vector3.one;
    }

    public void CreatGamePanel()
    {
        FadePanel(1);
        currentIndex = 1;
        if (currentStartPanel != null)//如果存在开始界面
        {
            Destroy(currentStartPanel);
        }
        if (currentGamePanel != null)//如果存在游戏场景
        {
            Destroy(currentGamePanel);
        }
        currentGamePanel = Instantiate(gamePanel) as GameObject;
        currentGamePanel.GetComponent<GamePanelControl>().InitGamePanel(canvas);
        currentGamePanel.transform.SetParent(transform);
        currentGamePanel.transform.localPosition = Vector3.zero;
        currentGamePanel.transform.localScale = Vector3.one;
    }

	public void CreatGamePanelOne()
	{
		FadePanel(1);
		currentIndex = 2;
		if (currentStartPanel != null)//如果存在开始界面
		{
			Destroy(currentStartPanel);
		}
		if (currentGamePanelOne != null)//如果存在游戏场景
		{
			Destroy(currentGamePanelOne);
		}
		currentGamePanelOne = Instantiate(gamePanelone) as GameObject;
		currentGamePanelOne.GetComponent<GamePanelControlOne>().InitGamePanel(canvas);
		currentGamePanelOne.transform.SetParent(transform);
		currentGamePanelOne.transform.localPosition = Vector3.zero;
		currentGamePanelOne.transform.localScale = Vector3.one;
	}

    public void FadePanel(float time)
    {
        fadePanel.color = new Color(0, 0, 0, 1);
        fadePanel.gameObject.SetActive(true);
		fadePanel.DOFade(0, time);//Dotween中的DOFade方法
    }

    /*public void Exit()
    {
    	print ("结束游戏");
    	// 关闭编辑器游戏运行模式
		if (UnityEditor.EditorApplication.isPlaying) {
			UnityEditor.EditorApplication.isPlaying = false;
		} else {
			Application.Quit();
		}

    	//关闭应用程序
    	//Application.Quit();
    }*/
}
