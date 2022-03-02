using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    public Camera playerCam;       
    public override void Use()
    {
        Shoot();
    }

    private void Shoot()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        ray.origin = playerCam.transform.position;
        Debug.DrawRay(ray.origin,ray.direction * 10 ,Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hitted this " + hit.collider.gameObject);
        }
    }
}
