using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Contant
{
    public const int Typt_Boom = 0;
    public const int Typt_Apple = 1;
    public const int Typt_Banana = 4;
    public const int Typt_Strawberry = 7;
    public const int Typt_Durian = 10;
    public const int Typt_Kiwifruit = 13;
	public const int Typt_Mango = 16;
	public const int Typt_Pineapple = 19;
	public const int Typt_Watermelon = 22;


    public static int GetOneType()
    {
		int[] type = { Typt_Apple, Typt_Banana, Typt_Strawberry, Typt_Durian, Typt_Kiwifruit, Typt_Mango, Typt_Pineapple, Typt_Watermelon};
        int t = Random.Range(0, type.Length);
        return type[t];
    }

	public static int GetOneTypeOne()
	{
		int[] type = { Typt_Boom, Typt_Apple, Typt_Banana, Typt_Strawberry};
		int t = Random.Range(0, type.Length);
		return type[t];
	}

}
public class Fruit : MonoBehaviour
{
    public int type = 0;
    public Sprite[] fruitSprites;
    public GameObject partical;

    private Image img;
    public Image Img
    {
        get
        {
            if (img == null)
            {
                img = this.GetComponent<Image>();
            }
            return img;
        }
    }
    float rot;
    void Start()
    {
        //随机一个旋转
        rot = Random.Range(-180, 180);
    }

    void LateUpdate()
    {
        transform.Rotate(Vector3.forward, rot * Time.deltaTime);
    }

    public void SetType(int type)
    {
        this.type = type;
        Img.sprite = fruitSprites[type];
        Img.SetNativeSize();
        if (type == Contant.Typt_Boom)
        {
            partical.SetActive(true);
        }
        else
        {
            partical.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //如果是被切掉的水果，则碰撞到边界时销毁
        if (col.CompareTag("boder"))
        {
            Destroy(this.gameObject);
        }
    }
}
