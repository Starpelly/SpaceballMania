using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Animator anim;
    public AnimationClip ballThrowClip;
    public float animSpeed;

    public Transform sprite;

    public AudioClip fall;

    public SpriteRenderer ballSprite;

    public GameObject dust;

    void Start()
    {
        // StartCoroutine(ThrowBall(this.transform.position, new Vector3(1.571f, -3.641f, 0), animSpeed / 2));
        Destroy(this.gameObject, 5f);
        Quaternion newRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        sprite.rotation = newRotation;
        anim.speed = animSpeed;
    }
    public void Delete()
    {
        Instantiate(dust);
        Destroy(this.gameObject);
    }

    IEnumerator ThrowBall(Vector3 valueToLerp, Vector3 endValue, float lerpDuration = 1.4f)
    {
        float timeElapsed = 0;
        Vector3 originalBallPos = new Vector3(-3.38f, -0.7f, 0);

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
}
