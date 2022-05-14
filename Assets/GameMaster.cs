using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMaster : MonoBehaviour
{

    private int activePalette;
    public Palettes[] palettes;

    void Start()
    {
        activePalette = Random.Range(0, palettes.Length);
        UpdatePalette();
    }
    [ContextMenu("Update Palette")]
    public void UpdatePalette()
    {
        Camera.main.backgroundColor = palettes[activePalette].background;
        SpriteRenderer[] components = GameObject.FindObjectsOfType<SpriteRenderer>();
        foreach(SpriteRenderer sr in components)
        {
            sr.color = palettes[activePalette].foreground;
        }
        GameObject.FindObjectOfType<Tilemap>().color = palettes[activePalette].foreground;
        GameObject.FindObjectOfType<ParticleSystem>().startColor = palettes[activePalette].foreground;
    }
}

[System.Serializable]
public class Palettes
{
    public Color32 foreground;
    public Color32 background;
}
