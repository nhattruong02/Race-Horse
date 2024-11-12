using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    [SerializeField] new string name;
    [SerializeField] float maxSpeedRD;
    [SerializeField] float minSpeedRD;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] float speedDown;
    private Vector3 movement;
    private float speed;
    bool isFinished;

    public string Name { get => name; private set => name = value; }
    public bool IsFinished { get => isFinished; set => isFinished = value; }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger(Common.Run);
        movement = Vector3.forward;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0, 0, movement.z);
    }

    private void FixedUpdate()
    {
        if (!isFinished)
        {
            if (GameManager.Instance.RemainDistance % 100 == 0 && GameManager.Instance.RemainDistance >= 0)
            {
                speed = Random.Range(minSpeedRD, maxSpeedRD);

            }
        }
        rb.velocity = movement * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Common.Finish))
        {
            isFinished = true;
        }
        StartCoroutine(finishedRace());
    }

    IEnumerator finishedRace()
    {
        yield return new WaitForSeconds(1);
        while (speed >= 0)
        {
            speed -= speedDown;
            if (speed <= 0.2f)
            {
                animator.SetTrigger(Common.Run);
                break;
            }
        }
    }

}
