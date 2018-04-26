using UnityEngine;
using System.Collections;

public class BtnCut : MonoBehaviour
{
    public Rigidbody2D r1;
    public Rigidbody2D r2;
    void Start()
    {
		//AddForce函数给物体添加刚体效果并且脚本增加一个力。
		r1.AddForce(new Vector2(-500, 500));
        r2.AddForce(new Vector2(500, -500));
    }
}
