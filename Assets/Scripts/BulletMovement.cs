using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] Vector2 direction;

    public float _Speed   
    {
        get { return speed; }
        set { speed = value; }
    }

    public Vector2 _Direction   
    {
        get { return direction; }
        set { direction = value; }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction.normalized  * speed * Time.deltaTime);
    }

}
