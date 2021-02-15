using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTextureColor : MonoBehaviour
{
    private int width = 256; // largura
    private int height = 256; // altura
    public int xOffset = 0;
    public int yOffset = 0;
    private Renderer _renderer;
    //noise
    [Range(1,8)]
    public int scale = 5; // número que determina a que distância visualizar o noise;
    [Range(1,6)]
    public int octaves = 6; // o número de níveis de detalhe que você deseja que o ruído perlin tenha
    public float persistance = 0.5f; // número que determina o quanto cada oitava contribui para a forma geral (ajusta a amplitude).
    //public  
    public float lacunarity = 2f; 
    //color
    public Gradient _gradient;
    public Texture2D texture;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.mainTexture = GenerateTexture();
    }

    public void BtnRand()
    {
        xOffset = Random.Range(0, 99999);
        yOffset = Random.Range(0, 99999);
    }

    void Update()
    {
        _renderer.material.mainTexture = GenerateTexture();
    }
    
    Texture2D GenerateTexture()
    {
        texture = new Texture2D(width, height, TextureFormat.RGBA32, true);
        int centerX = width / 2;
        int centerY = height / 2;
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                float distanceX = (centerX - x) * (centerX - x);
                float distanceY = (centerY - y) * (centerY - y);

                float distanceToCenter = Mathf.Sqrt(distanceX + distanceY);

                distanceToCenter = distanceToCenter / (width/2);
                
                Color color = CalculateColor(x, y,distanceToCenter);
                
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        
        return texture;
    }
    Color CalculateColor(int x, int y, float distanceToCenter)
    {
        float xCoord = (float)x / width * scale + xOffset ;
        float yCoord = (float)y / height * scale + yOffset;

        float sample = GetPerlin(xCoord, yCoord) - distanceToCenter;
        Color color =  _gradient.Evaluate(sample);

        return color;
    }
    float GetPerlin(float x, float y)
    {
        float total = 0;
        float frequency = 1;// número que determina o quanto cada oitava contribui para a forma geral (ajusta a amplitude).
        float amplitude = 1; //  número que determina quantos detalhes são adicionados ou removidos a cada oitava (ajusta a frequência).
        float maxValue = 0; 
         
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistance;
            frequency *= lacunarity;
        }
        return total/maxValue;
    }
}