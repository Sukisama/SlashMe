using UnityEngine;
using System.Collections;

public class BoderTrigger : MonoBehaviour
{
    private int count;

	//2D碰撞器
    void OnCollisionEnter2D(Collision2D coll)
    {
        count++;
        Destroy(coll.collider.gameObject);
        if (count == 3)
        {
			if (GameController._instance.currentIndex == 1) {//选择了游戏场景
				print ("选择了游戏场景1");
				count = 0;
				GameController._instance.CreatGamePanel ();
			}

			else if (GameController._instance.currentIndex == 2) {//没有选择游戏场景继续创建开始菜单
				print ("选择了游戏场景2");
				count = 0;
				GameController._instance.CreatGamePanelOne ();
			}
			else {
				print ("结束游戏");
				count = 0;
				// 关闭编辑器游戏运行模式
				//UnityEditor.EditorApplication.isPlaying = false;
				Application.Quit();
			}

        }
    }
}
