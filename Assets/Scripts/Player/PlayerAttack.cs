using UnityEngine;
using Enums;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement _player = null;

    [SerializeField]
    private AttackType _attackType;

    [SerializeField]
    private GameObject _hammerObj, _swordObj;
    private MeshCollider _weaponCollider;

    [SerializeField]
    private Spell _spell = null;
    public Spell Spell
    {
        get { return _spell; }
    }
    private GameObject _spellObj = null;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        switch (_attackType)
        {
            case AttackType.MeleeSword:
                _weaponCollider = _swordObj.GetComponent<MeshCollider>();
                _swordObj.SetActive(true);
                break;
            case AttackType.MeleeHammer:
                _weaponCollider = _hammerObj.GetComponent<MeshCollider>();
                _hammerObj.SetActive(true);
                break;
            case AttackType.Spell:
                SetupSpell();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (_attackType == AttackType.Spell)
        {
            if (_player.PlayerAnimator.GetBool("Casting"))
            {
                _player.PlayerAnimator.SetBool("Casting", false);
                _player.SetMovement(Movement.Free);
                _spellObj.SetActive(false);
            }
            else
            {
                CastSpell();
            }
        }
        else
        {
            _weaponCollider.enabled = true;
            _player.PlayerAnimator.SetTrigger("MeleeAttack");
        }
    }


    private void SetupSpell()
    {
        if (_spell)
        {
            if (_spell.SpellPrefab)
            {
                if (_spell.SpellOrigin == SpellOrigin.World)
                {
                    _spellObj = Instantiate(_spell.SpellPrefab, Vector3.zero, Quaternion.identity);
                    _spellObj.GetComponentInChildren<Weapon>().Owner = this;
                }
                else
                {
                    _spellObj = Instantiate(_spell.SpellPrefab, transform);
                }

                _spellObj.SetActive(false);
            }
        }
    }

    public void CastSpell()
    {
        if (!_spellObj) return;

        switch (_spell.SpellType)
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            _spellObj.transform.position = hit.point + Vector3.up * 5;
            _player.PlayerAnimator.SetTrigger("RangeAttack");
        }
    }

    public void CastContinuousSpell()
    {
        _player.PlayerAnimator.SetTrigger("RangeAttack");
        _player.PlayerAnimator.SetBool("Casting", true);
    }

    private void ToggleSpellVisibility()
    {
        if (_spellObj.activeSelf)
        {
            var particleCollision = _spellObj.transform.GetChild(0).GetComponent<ParticleSystem>().collision;
            particleCollision.sendCollisionMessages = true;
            _spellObj.SetActive(false);

        }
        else
        {
            _spellObj.SetActive(true);
        }
    }

    private void ToggleWeaponCollider()
    {
        _weaponCollider.enabled = !_weaponCollider.enabled;
    }
}
