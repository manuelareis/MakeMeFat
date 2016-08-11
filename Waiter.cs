using UnityEngine;
using System.Collections;

public class Waiter : MonoBehaviour
{

    [HideInInspector]
    static public Waiter waiterinstance;
    [SerializeField]
    Vector3 initialPosition = new Vector3(-19f, -5f, 0f);
    [SerializeField]
    GameObject flyingThings;
    [SerializeField]
    GameObject flyingPosition;
    Animator aninWaiter;
    bool walk;
    bool falled;
    float speed;

    void Awake()
    {
        if (waiterinstance != null)
            Destroy(gameObject);
        else
            waiterinstance = this;
    }
    void Start()
    {
        speed = 10;
        aninWaiter = GetComponentInChildren<Animator>();
        transform.position = initialPosition;
    }
    /// <summary>
    /// Reset the waiter to initial position with the walking animation
    /// </summary>
    void ResetWaiter()
    {
        speed = 10;
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
            if (!falled && transform.position.x > -2)
            {
                speed = 14;
                StartCoroutine("fallWaiter");

                falled = true;
                print("garcom caiu ");
            }
        }
    }
    /// <summary>
    /// Waiter Starts walkings
    /// </summary>
    public void StartWalk()
    {
        walk = true;
    }
    /// <summary>
    /// Waiter falling animation starts, after 1.5 sec he is reseted to initial position
    /// </summary>
    /// <returns></returns>
    public IEnumerator fallWaiter()
    {
        aninWaiter.SetBool("fall", true);
        PlayerScript.playerInstance.MoveDown();
        StartCoroutine("FlyFood");
        yield return new WaitForSeconds(1.2f);
        walk = false;
        GM.gmInstance.StartGame();
        ResetWaiter();
    }

    public IEnumerator FlyFood()
    {
        yield return new WaitForSeconds(0.25f);
        flyingThings = (GameObject)Instantiate(flyingThings, flyingPosition.transform.position, Quaternion.identity);
    }
    /// <summary>
    /// PingPong somethign in aValue speed from the aMin to aMax
    /// </summary>
    /// <returns></returns>
    float PingPong(float aValue, float aMin, float aMax)
    {
        return Mathf.PingPong(aValue, aMax - aMin) + aMin;
    }
}
