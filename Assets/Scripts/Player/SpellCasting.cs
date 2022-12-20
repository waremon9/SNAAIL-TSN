using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class SpellCasting : MonoBehaviour
{
    private PlayerMovement _player = null;

    [SerializeField]
    private Spell _spell = null;

    private GameObject spellObj = null;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (_spell)
        {
            if (_spell.spellPrefab)
            {
                if(_spell.spellOrigin == SpellOrigin.World)
                {
                    spellObj = Instantiate(_spell.spellPrefab, Vector3.zero, Quaternion.identity);
                } else
                {
                    spellObj = Instantiate(_spell.spellPrefab, transform);
                }
                spellObj.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CastSpell();
        }
    }

    public void CastSpell()
    {
        if (!spellObj) return;

        switch (_spell.spellType)
        {
            case SpellType.Continuous:
                CastContinuousSpell();
                break;
            case SpellType.Point:
                CastPointSpell();
                break;
            default:
                break;
        }
    }

    public void CastPointSpell()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            spellObj.transform.position = hit.point + Vector3.up * 5;
            spellObj.SetActive(true);
            _player.PlayerAnimator.SetTrigger("Cast");
        }
    }

    public void CastContinuousSpell()
    {
        spellObj.SetActive(true);
        _player.PlayerAnimator.SetBool("Casting", true);
    }
}
