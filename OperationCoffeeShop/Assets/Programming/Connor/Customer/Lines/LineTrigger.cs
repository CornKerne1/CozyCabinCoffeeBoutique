using UnityEngine;

public class LineTrigger : MonoBehaviour
{
    private CustomerLine _customerLine;

    [SerializeField] private string customerTag = "Customer";

    // Start is called before the first frame update
    private void Start()
    {
        _customerLine = GetComponentInParent<CustomerLine>();
    }

    private void OnTriggerEnter(Collider possibleCustomer)
    {
        if (possibleCustomer.tag.Equals(customerTag)) {
            _customerLine.GetInLine(possibleCustomer.gameObject);
        }
    }
}
