using UnityEngine;
using System.Collections;

public class Waiter : MonoBehaviour
{

    public float speed = 5;

    private Animator aninWaiter;
    static public Waiter waiterinstance;
    Rigidbody2D waiterrb;

    float rot = 90.0f;
    Vector3 v3 = Vector3.zero;

    bool isfacingright = true;

    Transform waiterTransf;

    void Start()
    {
        if (waiterinstance != null)
            Destroy(gameObject);
        else
            waiterinstance = this;

        waiterrb = GetComponentInChildren<Rigidbody2D>();
        aninWaiter = GetComponentInChildren<Animator>();
        waiterTransf = GetComponent<Transform>();


    }

    void Update()
    {
        //transform.position = new Vector3(PingPong(Time.time * speed, -15, 15), -1.5f, 0f);
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        if (transform.position.x < -12f && isfacingright == false || transform.position.x > 12f && isfacingright)
        {
            isfacingright = !isfacingright;
            //prender.flipX = !prender.flipX;
            waiterTransf.localScale = new Vector3(waiterTransf.localScale.x * -1, 1, 1);
        }
    }

    public void fallWaiter()
    {
        aninWaiter.SetBool("fall", true);
        Destroy(gameObject, 1.5f);
    }

    float PingPong(float aValue, float aMin, float aMax)
    {
        return Mathf.PingPong(aValue, aMax - aMin) + aMin;

    }
}
