using UnityEngine;
using Enums;
[CreateAssetMenu(menuName = "Spell")]
public class Spell : ScriptableObject
{
    public GameObject SpellPrefab;
    public SpellType SpellType;
    public SpellOrigin SpellOrigin;
    public int Damage;
}
