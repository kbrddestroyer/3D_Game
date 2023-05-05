using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    private void OnMouseOver()
    {
        if (player != null && Input.GetKeyDown(KeyCode.E))
        {
            player.PickupNut();
            Destroy(this.gameObject);
        }
    }
}
