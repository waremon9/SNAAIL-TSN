using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int _damage = 5;
    public int Damage { get { return _damage; } }
}
