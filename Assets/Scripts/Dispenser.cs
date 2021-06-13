using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public static Dispenser Instance { get; set; }

    public GameObject Ball;
    public GameObject HighBall;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Dispenser.Instance = this;
    }

    public void Shoot(float secPerBeat, Sprite sprite)
    {
        anim.Play("Shoot", 0);
        GameObject ball = Instantiate(Ball);
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.ballSprite.sprite = sprite;
        ballScript.animSpeed = secPerBeat;
    }

    public void ShootHigh(float secPerBeat, Sprite sprite)
    {
        anim.Play("Shoot", 0);
        GameObject ball = Instantiate(HighBall);
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.ballSprite.sprite = sprite;
        ballScript.animSpeed = secPerBeat;
    }
}
