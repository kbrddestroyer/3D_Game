using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : MonoBehaviour
{
    [SerializeField] private float spotRadius;

    private NavMeshAgent agent;
    private Player player;

    private bool hit = false;
    private float hp = 1f;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hit = true;
            agent.destination = transform.forward * -2;
        }
        if (collision.gameObject.tag == "Orb")
            hp -= 0.1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, spotRadius);
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();  
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            Destroy(this.gameObject);
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) < spotRadius && !hit)
            {
                agent.destination = player.transform.position;
            }

            if (agent.hasPath)
            {
                float angle = Vector3.Angle(transform.forward, agent.destination - transform.position);
                transform.Rotate(0, angle * Time.deltaTime * 2, 0);
            }
            else
            {
                hit = false;
                agent.destination = transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            }
        }
    }
}
