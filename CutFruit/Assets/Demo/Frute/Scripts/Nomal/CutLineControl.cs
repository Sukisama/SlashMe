using UnityEngine;
using System.Collections;

/// 切线控制

public class CutLineControl : MonoBehaviour
{

    /// 切线更新
    public void UpdateCutLine(Vector2 vec)
    {
        RectTransform rt = this.transform as RectTransform;

        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, vec, Time.deltaTime * 10);
    }
}
