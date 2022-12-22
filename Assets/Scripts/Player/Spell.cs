using UnityEngine;
using Enums;
[CreateAssetMenu(menuName = "Spell")]
public class Spell : ScriptableObject
{
    public GameObject spellPrefab;
    public SpellType spellType;
    public SpellOrigin spellOrigin;
}
