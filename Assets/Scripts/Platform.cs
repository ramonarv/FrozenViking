using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < player.transform.position.y - transform.localScale.y - player.transform.localScale.y - 0.5)
        {
            gameObject.layer = LayerMask.NameToLayer("PlatformActive");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("PlatformInactive");
        }
    }
}
