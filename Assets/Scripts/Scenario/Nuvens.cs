using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Nuvens : MonoBehaviour
{
    public List<Sprite> spriteList = new List<Sprite>();
    public List<GameObject> gameObjects = new List<GameObject>();
    public List<Transform> stationList = new List<Transform>();
    public GameObject spawn;
    public int spawnLimit = 5;
    private float timer = 2f;
    private float waitTimer;
    public float speed = 3;
    public int arrayEnd;
    public float minTimer, maxTimer;
    public float limitL, limitR;
    delegate void NuvensDelegate();
    static NuvensDelegate nuvensDelegate;
    public delegate void RemovalDelegate(GameObject obj);
    public static RemovalDelegate removalDelegate;
      
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (gameObject.tag == "Clouds")
        {
            nuvensDelegate = CloudSpawn;
        }
        else if (gameObject.tag == "Araras")
        {
            nuvensDelegate = AraraSpawn;
        }
        else if (gameObject.tag == "On�as")
        {
            nuvensDelegate = OncaSpawn;
        } else if (gameObject.tag == "Npc")
        {
           nuvensDelegate = NpcSpawn;
        }
        removalDelegate = PickOff;
    }
    private void OnDisable()
    {
        nuvensDelegate = null;
        removalDelegate=null;
    }

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        waitTimer += Time.deltaTime;
        nuvensDelegate?.Invoke();

    }

    private void CloudSpawn()
    {
        int rand = Random.Range(0, arrayEnd);
        spawn.GetComponent<SpriteRenderer>().sprite = spriteList[rand];
        if (gameObjects.Count < spawnLimit && waitTimer >= timer)
        {
            Vector3 nuvemV = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + Random.Range(-1f, 1f),
                gameObject.transform.position.z);

            gameObjects.Add(Instantiate(spawn, nuvemV, transform.rotation));
            waitTimer = 0f;
            timer = Random.Range(minTimer, maxTimer);
        }
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].transform.position.x > gameObject.transform.position.x - 30 && gameObjects[i].transform.position.x <= gameObject.transform.position.x + 3)
            {
                gameObjects[i].transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObjects[i]);
                gameObjects.Remove(gameObjects[i]);

            }
        }


    }
    private void AraraSpawn()
    {
        if (gameObjects.Count < spawnLimit && waitTimer >= timer)
        {
            Vector3 araraV = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + Random.Range(-1f, 1f),
                0);

            gameObjects.Add(Instantiate(spawn, araraV, transform.rotation));
            waitTimer = 0f;
            timer = Random.Range(minTimer, maxTimer);
        }
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].transform.position.x < gameObject.transform.position.x + limitR && gameObjects[i].transform.position.x > gameObject.transform.position.x - limitL)
            {
                gameObjects[i].transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObjects[i]);
                gameObjects.Remove(gameObjects[i]);

            }
        }
    }
    private void OncaSpawn()
    {
        if (gameObjects.Count < spawnLimit && waitTimer >= timer)
        {           

            gameObjects.Add(Instantiate(spawn, transform.position, transform.rotation));
            waitTimer = 0f;
            timer = Random.Range(minTimer, maxTimer);
        }
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].transform.position.x < gameObject.transform.position.x + 240)
            {
                
            }
            else
            {
                Destroy(gameObjects[i]);
                gameObjects.Remove(gameObjects[i]);

            }
        }
    }
    private void NpcSpawn()
    {
        if (gameObjects.Count < spawnLimit && waitTimer >= timer)
        {
            Vector3 point1 = stationList[Random.Range(0, stationList.Count)].position;
            GameObject obj = Instantiate(spawn, point1, transform.rotation);
            obj.GetComponentInChildren<Onças>().targets = stationList;
            obj.GetComponentInChildren<Onças>().speed = speed;
            gameObjects.Add(obj);
            waitTimer = 0f;
            timer = Random.Range(minTimer, maxTimer);
        }
    }

    private void PickOff(GameObject obj)
    {
        if (obj != null)
        {
            gameObjects.Remove(obj);
            Destroy(obj);
        }
    }
}
