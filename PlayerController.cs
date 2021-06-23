using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public Animator anim; 
    public Collider2D coll;
    public LayerMask ground;
    public AudioSource source;
    public AudioSource point;
    public Text pointText;
    public Text lifeText;

    public int speed;
    public int cherries = 0;
    public int jumpSpeed;
    public int hurtSpeed;
    public int life;
    public bool canHit;

    public enum State {idle, running, jumping, falling, hurt}
    public State state = State.idle;

    void Start() {
        speed = 5;
        life = 3;
        jumpSpeed = 15;
        hurtSpeed = 10;
        canHit = true;
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        pointText.text = "0";
        lifeText.text = "3";
    }

    // Update is called once per frame
    void Update()
    {   
        transform.eulerAngles = new Vector3 (0, 0, 0);
        if (state != State.hurt) {
            Movement();
        }
        VelocityChange();
        anim.SetInteger("States", (int)state);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Gem"){
            Destroy(other.gameObject);
            cherries += 1;
            if (cherries == 100){
                life += 1;
                lifeText.text = life.ToString();
            }
            point.Play();
            pointText.text = cherries.ToString(); 
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy"){
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (state == State.falling){
                canHit = false;
                enemy.Die();
                Jump();
                canHit = true;
            } else {
                if (canHit){
                    if (life != 0) {
                        life -= 1;
                        lifeText.text = life.ToString();
                    }
                    
                    state = State.hurt;

                    if (life == 0){
                        SceneManager.LoadScene("Scene1");
                    }                    
                    
                    if (other.gameObject.transform.position.x > transform.position.x){
                        rb.velocity = new Vector2(-hurtSpeed, rb.velocity.y);
                    } else {
                        rb.velocity = new Vector2(hurtSpeed, rb.velocity.y);
                    }
                }
                
            }
        }
    }

    public void Movement() {
        if (Input.GetKey(KeyCode.RightArrow)){   
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);   
        } else if (Input.GetKey(KeyCode.LeftArrow)){
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1); 
        }
    
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) {
            rb.velocity = new Vector2(rb.velocity.x,jumpSpeed);
            state = State.jumping;
            source.Play();
        }
    }

    public void VelocityChange() {
        if (state == State.jumping) {
            if (rb.velocity.y < 0.1f){
                state = State.falling;
            }
        } 
        else if (state == State.falling){
            if (coll.IsTouchingLayers(ground)){
                state = State.idle;
            }
        } 
        else if (state == State.hurt){
            if (Mathf.Abs(rb.velocity.x) < 0.1f){
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f){
            state = State.running;
        } 
        else {
            state = State.idle;
        }
    }

    public void StopHurt() {
        canHit = false;
    }

    public void StartHurt() {
        canHit = true;
    }

    public void Jump() {
        rb.velocity = new Vector2(rb.velocity.x,jumpSpeed);
        state = State.jumping;
        source.Play();
    }
}
