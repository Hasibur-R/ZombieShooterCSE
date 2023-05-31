using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{

    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance { get { return instance; } }

    public GameObject bulletPrefab;
    public int bulletAmount = 20;

    private List<GameObject> bullets;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //preload bulltes
        bullets = new List<GameObject>(bulletAmount);

        //bullet loops
        for(int i=0; i<bulletAmount; i++)
        {
            GameObject prefabInstance = Instantiate(bulletPrefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);


            bullets.Add(prefabInstance);
        }
    }

    // Update is called once per frame
    public GameObject GetBullet (bool shotByPlayer){
       foreach(GameObject bullet in bullets){
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
                return bullet;
            }
        }
        GameObject prefabInstance = Instantiate(bulletPrefab);
        prefabInstance.transform.SetParent(transform);
        prefabInstance.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;//2min 13 no
        bullets.Add(prefabInstance);

        return prefabInstance;
    }
}
