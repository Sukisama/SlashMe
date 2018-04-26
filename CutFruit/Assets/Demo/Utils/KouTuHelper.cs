using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KouTuHelper : MonoBehaviour
{
    public RawImage img;
    void Update()
    {
        BackgroundRemovalManager backManager = BackgroundRemovalManager.Instance;

        if (backManager && backManager.IsBackgroundRemovalInitialized())
        {
            if (img && img.texture == null)
            {
                //img.texture = backManager.GetForegroundTex();
                img.texture = backManager.GetAlphaBodyTex();
            }
        }
    }
}
