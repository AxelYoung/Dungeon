using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour {

    public float camSpeed;
    public BoxCollider2D bounds;
    public GameObject enemy;
    public int enemyAmount;
	// Use this for initialization
	void Start () {
        bounds = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        print(bounds.bounds.extents.x);
        InstantiateObst();
	}
	
    public void InstantiateObst()
    {
        for(int i = 0; i < enemyAmount; i++)
        {
            GameObject curEnemy = Instantiate(enemy, new Vector2(Random.Range(-bounds.bounds.extents.x, bounds.bounds.extents.x), Random.Range(-bounds.bounds.extents.y + bounds.gameObject.transform.position.y, bounds.bounds.extents.y + bounds.gameObject.transform.position.y)), Quaternion.identity);
            float rand = Random.Range(0.5f, 3f);
            curEnemy.transform.localScale = new Vector2(rand, rand);
            curEnemy.transform.eulerAngles = new Vector3(0,0,Random.Range(0f,360f));
        }
    }

	// Update is called once per frame
	void Update () {
        transform.position += Vector3.up * Time.deltaTime * camSpeed;
        if(transform.position.normalized.y == bounds.gameObject.transform.position.normalized.y)
        {
            bounds.gameObject.transform.position = new Vector2(0, bounds.gameObject.transform.position.y + 16);
            InstantiateObst();
        }
	}
}
