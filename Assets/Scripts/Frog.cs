using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField, Range(0f, 10f)] private float jumpForce;
    [SerializeField, Range(0f, 10f)] private float jumpDelay;
    [SerializeField, Range(0f, 10f)] private float randomAspect;

    private Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Orb")
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (randomAspect > jumpDelay)
        {
            randomAspect = jumpDelay;

            Debug.LogWarning("Неверная настройка объекта " + this.gameObject.name + ": randomAspect не должен быть больше чем jumpDelay");
        }

        StartCoroutine(jumpCoroutine());
    }

    private IEnumerator jumpCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpDelay + Random.Range(-randomAspect, randomAspect));

            Vector3 direction = new Vector3(
                Random.Range(-1f, 1f),
                0.0f,
                Random.Range(-1f, 1f)
                );
            this.transform.Rotate(new Vector3(0, Vector3.Angle(transform.forward, direction) + 90, 0));
            rb.AddForce((Vector3.up + direction) * jumpForce * 100);
        }
    }
}
