using System.Collections;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [SerializeField] Collider _sword;
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] Inventory _inven;
    [SerializeField] GameUI _gameUI;

    [SerializeField] int _maxHp;
    [SerializeField] int _maxMp;
    Animator _animator;

    int _curHp;
    int _curMp;
    int _coin = 0;

    float _mp;
    bool _canHitted = true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _curHp = _maxHp;
        _curMp = _maxMp;
        _gameUI.ShowHp(_curHp, _maxHp);
        _gameUI.ShowMp(_curHp, _maxHp);
    }

    void Update()
    {
        Move();
        Attack();
        ChargeMp();
        Defend();
        Jump();
    }

    void Move()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(_cam.transform.forward.x, 0, _cam.transform.forward.z));
        float vX = Input.GetAxisRaw("Horizontal");
        float vZ = Input.GetAxisRaw("Vertical");
        float vY = GetComponent<Rigidbody>().velocity.y;


        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 v3 = forward * vZ + right * vX;
        Vector3 vYz = v3.normalized * 4.5f;
        vYz.y += vY;

        if (Input.GetKey(KeyCode.LeftShift))
            vYz *= 2f;

        GetComponent<Rigidbody>().velocity = vYz;
        _animator.SetFloat("AxisX", vX);
        _animator.SetFloat("AxisZ", vZ);
        //if (v3 != Vector3.zero)
        //    transform.rotation = Quaternion.LookRotation(v3);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && _curMp > 0)
        {
            _curMp -= 5;
            _gameUI.ShowMp(_curMp, _maxMp);
            _sword.enabled = true;
            _animator.SetTrigger("doAttack");
        }
    }

    void EndAttack()
    {
        _sword.enabled = false;
    }

    void ChargeMp()
    {
        if (_curMp >= _maxMp)
            return;
        _mp += Time.deltaTime;
        if ((int)_mp >= 1)
        {
            _curMp += (int)_mp;
            _gameUI.ShowMp(_curMp, _maxMp);
            _mp = 0;
        }
    }

    void Defend()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetTrigger("doHit");
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("doJump");
        }
    }

    public void AddCoin()
    {
        Item item = new Item();
        int count = Random.Range(1, 100);
        EItemType eType = (EItemType)Random.Range(1, (int)EItemType.Max);
        item._eType = eType;
        item._count = count;
        _inven.AddItem(item);
        Debug.Log(eType);

    }

    public void Hitted()
    {
        if (!_canHitted)
            return;
        _curHp--;
        _gameUI.ShowHp(_curHp, _maxHp);
        if (_curHp <= 0)
        {
            _animator.SetTrigger("doDie");
        }
        else
        {
            _animator.SetTrigger("doHit");
        }
        _canHitted = false;
        StartCoroutine(HittedCoolTimeRoutine());
    }

    void DieEnd()
    {
        _gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    IEnumerator HittedCoolTimeRoutine()
    {
        yield return new WaitForSeconds(1f);
        _canHitted = true;
    }
}
