using System.Collections;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public Transform target;
    private float leftClamp, rightClamp, upClamp, downClamp;
    public float leftLimit, rightLimit, upLimit, downLimit;
    private bool isMoving = false;
    private float scale;
    public float speed;
    private int rand;
    private Vector3 previousPos;
    public bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {

        scale = gameObject.transform.localScale.x;

      leftClamp = gameObject.transform.position.x - leftLimit;
      rightClamp = gameObject.transform.position.x + rightLimit;
      upClamp = gameObject.transform.position.y + upLimit;
      downClamp = gameObject.transform.position.y - downLimit;
        PickDirection();  
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Moving();
        }
    }

    private void Moving()
    {
        target.position = new Vector3(
             Mathf.Clamp(target.position.x, leftClamp, rightClamp),
             Mathf.Clamp(target.position.y, downClamp, upClamp),
             transform.position.z);
        if (target.position.x < gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(-scale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (target.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(scale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.position, speed * Time.deltaTime);

        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(Wait(2f));
        }
    }
    private IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
       PickDirection();

        isMoving = false;
    }
    private void PickDirection()
    {
        previousPos = transform.position;
        rand = Random.Range(0,2);
        if (rand == 0)
        {
            target.position = new Vector3(Random.Range(gameObject.transform.position.x - 3, gameObject.transform.position.x + 3), gameObject.transform.position.y, gameObject.transform.position.z);
        } else
        {
            target.position = new Vector3(gameObject.transform.position.x, Random.Range(gameObject.transform.position.y - 3, gameObject.transform.position.y + 3), gameObject.transform.position.z);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
            target.position = previousPos;
    }
}
