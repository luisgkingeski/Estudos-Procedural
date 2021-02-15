using UnityEngine;
using System.Collections;

public class MergeTexures : MonoBehaviour
{

    private int width = 256; // largura
    private int height = 256; // altura
    public Gradient _gradient;

    public Texture2D CombineTexures(Texture2D tex1, Texture2D tex2)
    {
        Texture2D tex = new Texture2D(width, height);

        Color[] cols1 = tex1.GetPixels();
        Color[] cols2 = tex2.GetPixels();
       
        for (int i = 0; i < cols1.Length; ++i)
        {
           
                cols1[i].r = _gradient.Evaluate((cols1[i].r + cols2[i].r)/2).r;
                cols1[i].g = _gradient.Evaluate((cols1[i].g + cols2[i].r)/2).g;
                cols1[i].b = _gradient.Evaluate((cols1[i].b + cols2[i].r)/2).b;
            


        }

        tex.SetPixels(cols1);
        tex.Apply();
        return tex;
    }





}
