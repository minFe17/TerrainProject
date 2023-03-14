using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Monster>() != null)
        {
            other.GetComponent<Monster>().Hitted();
        }
    }
}
