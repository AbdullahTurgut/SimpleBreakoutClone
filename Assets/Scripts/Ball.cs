using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    // hız 
    float _speed = 20f;
    Rigidbody _rigidbody;
    Vector3 _velocity;

    //topun ekrandan çıktığında olucaklar
    Renderer _renderer;
    
    void Start()
    {
        //topun rigidbody componentine ulaştık
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
    }

    void Launch()
    {
        //topu başlangıçta down ile aşağı değil yukarı attık ki oyun başladığında hemen düşmesini kontrol edememeyi ortadan kaldırdık.  
        // ve delay ekliycez
        _rigidbody.velocity = Vector3.up * _speed;
    }
     
    void FixedUpdate()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
        _velocity = _rigidbody.velocity;
        //ekrandan çıktığını anlamak için
        if (!_renderer.isVisible)
        {
            //destroy etmeden önce gamemanagere ulaşıp bildiriyoruz
            GameManager.Instance.Balls--;
            //topu yok ettik
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //topun paddle çarptıktan sonra bir ivme ile sekmesini sağlamak için
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }
}
