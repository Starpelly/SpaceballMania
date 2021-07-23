using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        // StartCoroutine(autoPlay(1));

        // Player.instance.hitTimes.Add(Conductor.instance.songPosBeat + 1);
    }

    public void ShootHigh(float secPerBeat, Sprite sprite)
    {
        anim.Play("Shoot", 0);
        GameObject ball = Instantiate(HighBall);
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.ballSprite.sprite = sprite;
        ballScript.animSpeed = secPerBeat;

        // StartCoroutine(autoPlay(2));

        // Player.instance.hitTimes.Add(Conductor.instance.songPosBeat + 2);
    }

    IEnumerator autoPlay(int multiply)
    {
        yield return new WaitForSeconds(Conductor.instance.secPerRealBeat * multiply);
        Player.instance.Swing();
        Debug.Log(Conductor.instance.beatPosition);
    }
}
