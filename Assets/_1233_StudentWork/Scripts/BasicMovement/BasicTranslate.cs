using UnityEngine;

public class BasicTranslate : MonoBehaviour
{
    [SerializeField] private Vector3 _speed;
    [SerializeField] private bool _fixedUpdate; // [ true / false ]

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation:_speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(translation: _speed * Time.fixedDeltaTime);
    }
}
