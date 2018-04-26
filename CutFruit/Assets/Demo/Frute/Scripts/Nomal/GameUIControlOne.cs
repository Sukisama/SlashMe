using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIControlOne : MonoBehaviour
{
    public Text allLife;
    public Text score;
    public GameObject gameOver;

	public void SetScore(int sc, int life)
    {
        score.text = sc + "";
		if (life > 0) 
		{
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
