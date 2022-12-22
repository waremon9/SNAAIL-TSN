using System;
using UnityEngine;
using Enums;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private GameObject _spellObj;
    
    private PlayerControls _playerControls;
    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        _playerControls.Player.Fire.performed += Attack;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "GamePlayScene") return;
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

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    void Attack(InputAction.CallbackContext context)
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
            _player.PlayerAnimator.SetTrigger("MeleeAttack");
        }
    }


    private void SetupSpell()
    {
        if (!_spell)
            return;

        if (!_spell.SpellPrefab)
            return;
        
        if (_spell.SpellOrigin == SpellOrigin.World)
        {
            _spellObj = Instantiate(_spell.SpellPrefab, Vector3.zero, Quaternion.identity); 
			_spellObj.GetComponentInChildren<Weapon>().Owner = this;
        }
        else
        {
            _spellObj = Instantiate(_spell.SpellPrefab, _player._playerMesh.transform);
        }
        _spellObj.SetActive(false);
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
        if(Physics.Raycast(Camera.main.ScreenPointToRay(_player.GetMousePosition()), out hit))
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

    public void ToggleSpellVisibility()
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

    public void ToggleWeaponCollider()
    {
        _weaponCollider.enabled = !_weaponCollider.enabled;
    }
}
