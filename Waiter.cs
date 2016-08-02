using UnityEngine;
using System.Collections;

public class Waiter : MonoBehaviour
{

    [HideInInspector]
    static public Waiter waiterinstance;
    [SerializeField]
    float speed = 5;
    [SerializeField]
    Vector3 initialPosition = new Vector3(-13f, -1.5f, 0f);
    Animator aninWaiter;
    Rigidbody2D waiterrb;
    float rot = 90.0f;
    Vector3 v3 = Vector3.zero;
    bool isfacingright = true;
    bool walk;
    bool falled;
    Transform waiterTransf;

    void Awake()
    {
        if (waiterinstance != null)
            Destroy(gameObject);
        else
            waiterinstance = this;
    }
    void Start()
    {
        waiterrb = GetComponentInChildren<Rigidbody2D>();
        aninWaiter = GetComponentInChildren<Animator>();
        waiterTransf = GetComponent<Transform>();
        transform.position = initialPosition;
    }
    void ResetWaiter()
    {
        aninWaiter.SetBool("fall", false);
        transform.position = initialPosition;
        transform.localRotation = Quaternion.identity;
        walk = false;
        falled = false;
    }

    void Update()
    {
        if (walk)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            if (!falled && transform.position.x > -1)
            {
                StartCoroutine("fallWaiter");
                PlayerScript.playerInstance.MoveDown();
                GM.gmInstance.StartGame();
                falled = true;
            }
        }
    }

    public void StartWalk()
    {
        walk = true;
    }

    public void WaiterStartWalk()
    {
        InvokeRepeating("WalkWaiter", 0.1f, 0.1f);
    }

    public IEnumerator fallWaiter()
    {
        print("waiterfall");
        aninWaiter.SetBool("fall", true);
        yield return new WaitForSeconds(1.5f);
        walk = false;
        ResetWaiter();

    }

    float PingPong(float aValue, float aMin, float aMax)
    {
        return Mathf.PingPong(aValue, aMax - aMin) + aMin;
    }
}
