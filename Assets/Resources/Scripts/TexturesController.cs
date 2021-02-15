using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TexturesController : MonoBehaviour
{
    public GameObject BW;
    public GameObject CLR;
    
    public GameObject first, second, final;
    private List<Texture2D> images = new List<Texture2D>();
    private int index1, index2;
    private MergeTexures mergeTexures;



    private Renderer firstRend, secondRend, finalRend, selecionado;


    public void SelecionarFirst()
    {
        selecionado = firstRend;
    }

    public void SelecionarSecond()
    {
        selecionado = secondRend;
    }


    void Start()
    {
        index1 = index2 = 0;
        firstRend = first.GetComponent<Renderer>();
        secondRend = second.GetComponent<Renderer>();
        finalRend = final.GetComponent<Renderer>();
        selecionado = firstRend;
        mergeTexures = final.GetComponent<MergeTexures>();
    }


    public void BtnSaveImg()
    {
        SaveTextureAsPNG(BW.GetComponent<NoiseTextureBW>().texture);
        //SaveTextureAsPNG(CLR.GetComponent<NoiseTextureColor>().texture);
    }

    public void BtnLoadImg()
    {
        StartCoroutine(LoadAll(Directory.GetFiles(Application.persistentDataPath, "*.png", SearchOption.AllDirectories)));

    }

    public void BtnBackImg()
    {
        if (selecionado == firstRend)
        {
            index1--;
            if (index1 < 0)
            {
                index1 = images.Count - 1;
            }
            selecionado.material.mainTexture = images[index1];
        }
        else
        {
            index2--;
            if (index2 < 0)
            {
                index2 = images.Count - 1;
            }
            selecionado.material.mainTexture = images[index2];
        }
        finalRend.material.mainTexture = mergeTexures.CombineTexures(images[index1], images[index2]);
    }

    public void BtnNextImg()
    {
        if (selecionado == firstRend)
        {
            index1++;
            if (index1 >= images.Count)
            {
                index1 = 0;
            }

            selecionado.material.mainTexture = images[index1];

        }
        else
        {
            index2++;
            if (index2 >= images.Count)
            {
                index2 = 0;
            }

            selecionado.material.mainTexture = images[index2];
        }
        finalRend.material.mainTexture = mergeTexures.CombineTexures(images[index1], images[index2]);


    }

    private string GenerateImageCode()
    {
        string characters = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string name = "";

        for (int i = 0; i < 20; i++)
        {
            int a = Random.Range(0, characters.Length);
            name = name + characters[a];
        }

        return name + ".png";
    }



    public void SaveTextureAsPNG(Texture2D _texture)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        Destroy(_texture);
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, GenerateImageCode()), _bytes);

    }

    public IEnumerator LoadAll(string[] filePaths)
    {
        foreach (string filePath in filePaths)
        {
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
            WWW load = new WWW("file:///" + filePath);
#pragma warning restore CS0618 // O tipo ou membro é obsoleto
            yield return load;
            if (!string.IsNullOrEmpty(load.error))
            {
                Debug.LogWarning(filePath + " error");
            }
            else
            {
                images.Add(load.texture);

            }
        }


        firstRend.material.mainTexture = images[index1];
        secondRend.material.mainTexture = images[index2];
        finalRend.material.mainTexture = mergeTexures.CombineTexures(images[index1], images[index2]);
        


    }


}