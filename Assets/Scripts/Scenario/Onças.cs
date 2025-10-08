using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Onças : MonoBehaviour
{
    float c;
    public SpriteRenderer oncaSprite;
    public Transform target;
    public int limitL, limitR;
    public float speed, waitTime;
    private float scale;
    private bool waiting = false;
    public Animator anim;
    public float reach = 3;
    public SpriteLibraryAsset[] assets;
    public SpriteLibrary asset;
    public List<Transform> targets = new List<Transform>();
    
    void Start()
    {

        c = 0f;
        oncaSprite.color = new Color(1f, 1f, 1f, c);
        asset.spriteLibraryAsset = assets[Random.Range(0, assets.Length)];
        anim.SetBool("halt", false);
        anim.SetBool("move", true);
        scale = gameObject.transform.localScale.x;
        waiting = true;
        do
        {
            target.position = targets[Random.Range(0, targets.Count)].position;
        } while (target.position == transform.position);

            
        
        anim = gameObject.GetComponent<Animator>();
    }

    
    void Update()
    {
        if (waiting == true)
        {
            Fade();
        }
        if (waiting == false)
        {
            Movement();
        }

    }
    private void Fade()
    {
        if (c < 1)
        {
            c = Mathf.MoveTowards(c, 1, 0.01f);
            oncaSprite.color = new Color(1f, 1f, 1f, c);
        }
        if (c == 1f)
        {
            waiting = false;
        }
    }
    private void Movement()
    {
        

        transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, limitL, limitR),
                Mathf.Clamp(transform.position.y, limitL, limitR),
                transform.position.z);
            if (target.position.x < gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(scale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
            else if (target.position.x > gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(-scale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
            anim.SetBool("halt", false);
            anim.SetBool("move", true);
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.position, speed * Time.deltaTime);
            if (limitL > target.position.x || target.position.x > limitR)
            {
                target.position = gameObject.transform.position;
            }
            if (gameObject.transform.position.x == target.position.x && waiting == false)
            {
              c = Mathf.MoveTowards(c, 0, 0.01f);
            oncaSprite.color = new Color(1f, 1f, 1f, c);           
            if (c == 0f)
            {
                Nuvens.removalDelegate?.Invoke(this.gameObject.transform.parent.gameObject) ;
            }
             
            }
        
    }
    IEnumerator Wait (float secs)
    {
        anim.SetBool("move", false);
        anim.SetBool("halt", true);
        yield return new WaitForSeconds(secs);
        do
        {
            target.position = targets[Random.Range(0, targets.Count)].position;
        }while (target.position == gameObject.transform.position);
        
        waiting = false;
        anim.SetBool("halt", false);
        anim.SetBool("move", true);

    }
}
