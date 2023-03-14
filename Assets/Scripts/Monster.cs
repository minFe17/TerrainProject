using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject _monster;
    [SerializeField] Transform _target;
    [SerializeField] GameObject _attackSpace;
    [SerializeField] GameObject _coin;
    [SerializeField] float _searchDis;
    [SerializeField] float _attackDis;
    [SerializeField] float _speed;

    Animator _animator;
    EMonState _eState = EMonState.None;

    int _hp;
    bool _canhitted = true;

    void Start()
    {
        _animator = _monster.GetComponent<Animator>();
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (_eState == EMonState.Idle)
            MoveAndSearch();
        if (_eState == EMonState.Attack)
            FollowAndAttack();
        _attackSpace.SetActive(_eState == EMonState.Attack);
    }


    void MoveAndSearch()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Hitted") == false || _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            _animator.Play("Idle");

        }
        float dis = Vector3.Distance(_target.position, transform.position);
        if (dis < _searchDis)
        {
            if (dis < _attackDis)
            {
                _eState = EMonState.Attack;
            }
            else
            {
                Vector3 lookDir = _target.position - transform.position;
                transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
            }
        }
        else
        {
            _eState = EMonState.Move;
            StartCoroutine(RandomMoveRoutine());
        }
    }

    void FollowAndAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hitted") == false || _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            _animator.Play("Attack");
        }
        float dis = Vector3.Distance(_target.position, transform.position);
        if (dis < _searchDis)
        {
            if (dis > _attackDis)
            {
                Vector3 lookDir = _target.position - transform.position;
                transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
            }
        }
        else
        {
            _eState = EMonState.Idle;
        }

    }

    public void Hitted()
    {
        if (!_canhitted)
            return;
        _hp--;
        if (_hp <= 0)
        {
            _animator.Play("Die");
            _eState = EMonState.Die;
        }
        else
        {
            _animator.Play("Hitted");
        }
        _canhitted = false;
        StartCoroutine(HittedCoolTimeRoutine());
    }

    public void DieEnd()
    {
        _monster.gameObject.SetActive(false);
        GameObject temp = Instantiate(_coin);
        temp.transform.position = transform.position;
    }

    void Spawn()
    {
        _monster.SetActive(true);
        _eState = EMonState.Idle;
        _hp = 5;
    }

    IEnumerator RandomMoveRoutine()
    {
        bool canMove = false;
        RaycastHit hit;
        Vector3 targetDir = Vector3.zero;
        while (!canMove)
        {
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            transform.rotation = Quaternion.LookRotation(randomDir);

            yield return new WaitForSeconds(1f);
            targetDir = transform.position + transform.forward * 2;//movedir

            Vector3 front = transform.position + transform.forward + new Vector3(0, 1f, 0);

            Debug.DrawRay(front, transform.forward * 2, Color.red, 5);

            if (Physics.Raycast(front, transform.forward * 2, out hit, 2))
            {
                Debug.Log("hit collider : " + hit.collider.name);
            }
            else
            {
                canMove = true;
            }
        }

        Debug.DrawRay(targetDir + new Vector3(0, 0.5f, 0), new Vector3(0, -2f, 0), Color.red, 5);
        if (Physics.Raycast(targetDir + new Vector3(0, 0.5f, 0), new Vector3(0, -2f, 0), out hit))
        {
            while (Vector3.Distance(transform.position, targetDir) > 0.1)
            {
                _animator.Play("Move");
                transform.position = Vector3.MoveTowards(transform.position, targetDir, Time.deltaTime * _speed);
                yield return null;
            }
        }
        yield return new WaitForSeconds(1f);
        _eState = EMonState.Idle;
    }

    IEnumerator SpawnRoutine()
    {
        int rand = Random.Range(2, 5);
        yield return new WaitForSeconds(rand);
        Spawn();
    }

    IEnumerator HittedCoolTimeRoutine()
    {
        yield return new WaitForSeconds(1f);
        _canhitted = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero"))
        {
            other.GetComponent<CharacterMove>().Hitted();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hero"))
        {
            other.GetComponent<CharacterMove>().Hitted();
        }
    }
}

public enum EMonState
{
    None,
    Idle,
    Move,
    Attack,
    Die,
}
