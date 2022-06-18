using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hits = 1;
    public int points = 100;
    
    public Vector3 rotator;

    public Material hitMaterial;
    Material _orgMaterial;
    Renderer _renderer;
    void Start()
    {
        //brickleri döndürmek için 
        transform.Rotate(rotator * (transform.position.x + transform.position.y)*0.1f);
        //top bricklere çarptığında ilk bir beyazlık efekti vermek için renderer a eriştik.
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
    }

    
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);        
    }

    private void OnCollisionEnter(Collision collision)
    {
        hits--;
        //score points
        if (hits <= 0)
        {
            //gameManagerdaki ınstance'a eriştik.
            GameManager.Instance.Score += points;
            Destroy(gameObject);
        }

        //çarpma olduğunda çarpma materialını getirmek için
        _renderer.sharedMaterial = hitMaterial;
        Invoke("RestoreMaterial", 0.05f);
    }

    void RestoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }
}
