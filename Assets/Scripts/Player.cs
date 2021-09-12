using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rig;
    public SpriteRenderer spriteRenderer;
    private GameController gc;
    private float direction;

    public float helth;

    public float speed;
    public float jumptForce;

    bool jumping;
    public bool vulnerable;


    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        // retorna uma direção no eico x com vlaor entre -1 (esquerda) e 1 (direita)
        direction = Input.GetAxis("Horizontal");

        if(direction > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        if(direction < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }

        if(direction != 0 && jumping == false)
        {
            anim.SetInteger("transition", 1);
        }

        if(direction == 0 && jumping == false)
        {
            anim.SetInteger("transition", 0);
        }

        Jump();
    }

    public void FixedUpdate()
    {
        rig.velocity = new Vector2(direction * speed, rig.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumping == false)
        {
            rig.AddForce(Vector2.up * jumptForce, ForceMode2D.Impulse);
            anim.SetInteger("transition", 2);
            jumping = true;
        }
    }

    public void GenerateDamage()
    {
        if (vulnerable == false)
        {
            helth--;
            gc.lostLive(helth);
            vulnerable = true;
            StartCoroutine(Respawm());
        }

    }

    IEnumerator Respawm()
    {
        for (int i = 0; i < 15; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        vulnerable = false;
    }

    //método é chamado automaticamente pela unity quando o objeto toca em outro objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            jumping = false;
        }
    }

    
}
