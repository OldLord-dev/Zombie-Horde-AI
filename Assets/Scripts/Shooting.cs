using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject bullet = Pool.singleton.Get("Bullet");

            if (bullet != null)
            {
                BulletBody bulletBody = bullet.GetComponent<BulletBody>();
                bulletBody.velocity = new Vector2(direction.x, direction.y).normalized * bulletBody.force;

                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, rotZ-90.0f);
                bullet.SetActive(true);
            }
        }
    }
}
