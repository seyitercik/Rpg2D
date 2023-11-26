
using UnityEngine;
[CreateAssetMenu(fileName = "Thunder strike effect",menuName = "Data/Item Effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefabs;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefabs,_respawnPosition.position,Quaternion.identity);
        Destroy(newThunderStrike,1);

    }
}
