using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Particle : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = GameObject.FindObjectOfType<Tilemap>().color;
        StartCoroutine(DestroyParticle());
    }

    public IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
