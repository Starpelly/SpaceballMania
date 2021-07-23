using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnim : MonoBehaviour
{
    public Animator anim;

    public Rigidbody2D rb;
    public new Collider2D collider;

    public AudioSource audioSrc;
    public AudioClip miss;
    public AudioClip hit;

    bool hasMissed = false;
    bool hasHit = false;

    public Ball ballScript;

    public GameObject ballSpr;

    void Start()
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        ballSpr.transform.localRotation = newRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BallHolder")
        {
            return;
        }

        if (Player.instance.canHitAgain == true)
        {
            if (collision.tag == "Miss")
            {
                if (hasHit == true)
                    return;

                hasMissed = true;
                //StartCoroutine(HitBall(this.transform.position, new Vector3(-90, Random.Range(60, -60), 0), 1.2f));
                audioSrc.PlayOneShot(miss);
                collider.enabled = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForce(this.transform.up * 1100);
                rb.gravityScale = 9;
            }
            if (collision.tag == "Player")
            {
                if (hasMissed == true)
                    return;

                Player.instance.canHitAgain = false;
                hasHit = true;
                hasMissed = false;
                audioSrc.PlayOneShot(hit);
                collider.enabled = false;
                StartCoroutine(HitBall(this.transform.position, new Vector3(Random.Range(5, 25), 0, -600), 5f));
                // StartCoroutine(ResizeBall(this.transform.localScale, new Vector3(20, 20, 0), 5f));
                // StartCoroutine(RotateBall(this.transform.rotation, new Quaternion(0, 0, 670, 0)));
            }
            anim.enabled = false;
        }
    }

    IEnumerator HitBall(Vector3 valueToLerp, Vector3 endValue, float lerpDuration = 1.4f)
    {
        float timeElapsed = 0;
        Vector3 originalBallPos = this.transform.position;

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Vector3.Lerp(originalBallPos, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            this.transform.position = valueToLerp;
            yield return null;
        }
        this.transform.position = endValue;
        valueToLerp = endValue;
    }

    IEnumerator ResizeBall(Vector3 valueToLerp, Vector3 endValue, float lerpDuration = 1.4f)
    {
        float timeElapsed = 0;
        Vector3 originalBallSize = this.transform.localScale;

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Vector3.Lerp(originalBallSize, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            this.transform.localScale = valueToLerp;
            yield return null;
        }
        this.transform.localScale = endValue;
        valueToLerp = endValue;
    }

    IEnumerator RotateBall(Quaternion valueToLerp, Quaternion endValue, float lerpDuration = 1.4f)
    {
        float timeElapsed = 0;
        Quaternion originalBallSize = this.transform.rotation;

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Quaternion.Lerp(originalBallSize, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            this.transform.rotation = valueToLerp;
            yield return null;
        }
        this.transform.rotation = endValue;
        valueToLerp = endValue;
    }

}
