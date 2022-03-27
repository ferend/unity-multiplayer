
using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    public Camera playerCam;
    public PhotonView _pv;
    
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
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo) itemInfo).damageAmount);
            _pv.RPC(nameof(RPC_Shoot),RpcTarget.All,hit.point);
        }
    }

    [PunRPC]
    public void RPC_Shoot(Vector3 hitPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.2F);
        if (colliders.Length != 0)
        {
            GameObject playerBullet = Instantiate(bullet, hitPosition, Quaternion.identity);
            playerBullet.transform.SetParent(colliders[0].transform);
            Destroy(playerBullet, 2f);

        }
    }

  
}
