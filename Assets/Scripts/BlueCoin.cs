using UnityEngine;

public class BlueCoin : MonoBehaviour
{
    [SerializeField] GameObject _parent;
    [SerializeField] GameObject _effect;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void EndGetCoin()
    {
        _effect.SetActive(true);
    }

    void DestroyCoin()
    {
        Destroy(_parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero"))
        {
            other.GetComponent<CharacterMove>().AddCoin();
            _animator.SetBool("isGetCoin", true);
        }
    }
}
