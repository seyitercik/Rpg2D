
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Ice and fire effect",menuName = "Data/Item Effect/Ice and fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float XVelocity;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player.Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttcak.comboCounter == 2;
        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(XVelocity*player.facingDir,0);
            Destroy(newIceAndFire, 10);
        }
        
        
    }
}
