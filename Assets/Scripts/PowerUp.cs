using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    private Player _player;

    //ID for powerups
    //0 = tripleshot
    //1 = speed
    //2 = shield
    [SerializeField]
    private int powerUpId;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        Vector3 pos = transform.position;
        if (pos.y < -6.6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.transform.GetComponent<Player>();
            if(_player != null)
            {

                switch(powerUpId)
                {
                    case 0:
                        _player.TsActive();
                        _player.PlayPowerUpSound();
                        break;

                    case 1:
                        _player.SpeedActive();
                        _player.PlayPowerUpSound();
                        break;
                    case 2:
                        _player.ShieldActive();
                        _player.PlayPowerUpSound();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);

        }

    }
}
