using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    //0 = tripleshot
    //1 = speed
    //2 = shield
    [SerializeField]
    private int powerUpId;

    private bool _iscollected = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(
            Vector3.down * _speed * Time.deltaTime);

        Vector3 pos = transform.position;
        if (pos.y < -6.6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_iscollected) return;

        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Player component is missing!", this);
            return;
        }


        switch (powerUpId)
        {
            case 0:
                player.TsActive();
                break;

            case 1:
                player.SpeedActive();
                break;

            case 2:
                player.ShieldActive();
                break;

            default:
                Debug.LogError("Invalid PowerUp ID: " + powerUpId, this);
                return;
        }
        _iscollected = true;

        player.PlayPowerUpSound();


        Destroy(this.gameObject);



    }
}
