using PellyTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHolder : MonoBehaviour
{
	public GameObject eventHolder;

	public GameObject holder;
	void Update()
	{
		if (transform.localPosition.y > 1f)
		eventHolder.GetComponent<SpriteRenderer>().color = Colors.Hex2RGB("D94D4D");
		else if (transform.localPosition.y > 0f)
			eventHolder.GetComponent<SpriteRenderer>().color = Colors.Hex2RGB("BF9F3F");
		else if (transform.localPosition.y < 0f && transform.localPosition.y > -1f)
			eventHolder.GetComponent<SpriteRenderer>().color = Colors.Hex2RGB("3FBF5F");
		else if (transform.localPosition.y < -1f)
			eventHolder.GetComponent<SpriteRenderer>().color = Colors.Hex2RGB("3FBFBF");
		else
			eventHolder.GetComponent<SpriteRenderer>().color = Colors.Hex2RGB("D9D9D9");
	}
}
