using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIControl : MonoBehaviour
{
    public Text allTime;
    public Text score;
    public GameObject gameOver;

	public void SetScore(int sc, int time)
    {
        score.text = sc + "";
		if (time >= 0) 
		{
			allTime.text = time.ToString ();
		}
		else 
		{
			GameOver();
		}
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }
}
