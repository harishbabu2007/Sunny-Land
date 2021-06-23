using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{   
    public bool isLeft;
    public int speed;
    // Start is called before the first frame update

    protected override void Start() {
        base.Start();
        isLeft = true;
        speed = 5;
    }

    private void Update() {
        transform.eulerAngles = new Vector3 (0, 0, 0);
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Lpost"){
            isLeft = false;
        } else if (other.tag == "Rpost"){
            isLeft = true;
        }
    }

    public void Movement(){
        if (isLeft == true) {
            transform.localScale = new Vector3(1,1,1);
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        } else if (isLeft == false){
            transform.localScale = new Vector3(-1,1,1);
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }
}
