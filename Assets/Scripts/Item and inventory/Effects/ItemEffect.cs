
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data",menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _respawnPosition)
    {
        Debug.Log("Effect executed");
    }
}
