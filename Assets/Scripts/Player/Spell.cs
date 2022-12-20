using UnityEngine;

public enum SpellType
{
    Continuous,
    Point
}
public enum SpellOrigin
{
    Player,
    World
}

[CreateAssetMenu(menuName = "Spell")]
public class Spell : ScriptableObject
{
    public GameObject spellPrefab;
    public SpellType spellType;
    public SpellOrigin spellOrigin;
}
