using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Player : MonoBehaviour
{
    [Header("Настройка параметров игрока")]
    [SerializeField, Range(0f, 10f)]    private float speed;
    [SerializeField, Range(0f, 10f)]    private float sprintSpeed;
    [SerializeField, Range(0f, 10f)]    private float crouchSpeed;
    [SerializeField, Range(0f, 10f)]    private float jumpForce;
    [SerializeField, Range(0f, 10f)]    private float mouseSens;
    [SerializeField, Range(0f, 10f)]    private float orbInitialForce;
    [SerializeField, Range(0f, 2f)]     private float crouchHeight;
    [Header("Префабы зависимостей")]
    [SerializeField] private GameObject magicOrb;
    [SerializeField] private SceneAsset nextLevel;
    private Text nutsCounter;
    private Image damageFX;
    private Slider hpBar;
    private float normalHeight;
    private float hp = 1;
    public float HP { get { return hp; } }
    private bool isOnGround = true;
    private int nuts = 0, maxNuts;
    // Importain components
    private Animation           animator;
    private AudioSource         audioSource;
    private Rigidbody           rb;
    private new BoxCollider     collider;
    private new Camera          camera;
    private Vector2             rotation;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isOnGround = true;
        if (collision.gameObject.tag == "Enemy")
        {
            DealDamage();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        nutsCounter = GameObject.Find("NutsCounter").GetComponent<Text>();
        damageFX = GameObject.Find("DamageFX").GetComponent<Image>();
        hpBar = GameObject.Find("HP").GetComponent<Slider>();
        animator = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        normalHeight = collider.size.y;
        camera = Camera.main;
        rotation = Vector2.zero;

        maxNuts = GameObject.FindGameObjectsWithTag("Nut").Length;
        nutsCounter.text = nuts.ToString() + '/' + maxNuts.ToString();
    }

    private IEnumerator DamageEffect()
    {
        Color color = damageFX.color;
        for (float i = 0; i <= Mathf.PI; i += 0.25f)
        {
            yield return new WaitForSeconds(0.01f);
            color = damageFX.color;
            color.a = Mathf.Sin(i);
            damageFX.color = color;
        }
        color.a = 0f;
        damageFX.color = color;
    }

    private void DealDamage()
    {
        hp -= 0.1f;
        hpBar.value = hp;
        if (hp <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(DamageEffect());
    }

    public void PickupNut()
    {
        nuts++;
        nutsCounter.text = nuts.ToString() + '/' + maxNuts.ToString();

        if (nuts == maxNuts)
            SceneManager.LoadScene(nextLevel.name);
    }

    // Update is called once per frame
    void Update()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * mouseSens;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens;

        rotation.x -= mouseY;
        rotation.y += mouseX;
        rotation.x = Mathf.Clamp(rotation.x, - 89, 89);
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector3.up * jumpForce * 100);
            isOnGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject orb = Instantiate(magicOrb, camera.transform.position + camera.transform.forward, transform.rotation);
            orb.GetComponent<Rigidbody>().AddForce(camera.transform.forward * orbInitialForce * 100);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            collider.size = new Vector3(collider.size.x, normalHeight, collider.size.z);
            collider.center = new Vector3(collider.center.x, normalHeight / 2.0f - 1, collider.center.z);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;
            collider.size = new Vector3(collider.size.x, crouchHeight, collider.size.z);
            collider.center = new Vector3(collider.center.x, crouchHeight / 2.0f - 1, collider.center.z);
        }
        else
        {
            collider.size = new Vector3(collider.size.x, normalHeight, collider.size.z);
            collider.center = new Vector3(collider.center.x, normalHeight / 2.0f - 1, collider.center.z);
        }
        transform.Translate(new Vector3(axisX, 0, axisY) * currentSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        camera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, collider.size.y / 2, camera.transform.localPosition.z);
    }
}
