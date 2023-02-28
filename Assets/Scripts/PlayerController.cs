using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveHorizontal , 0,moveVertical);
        transform.rotation = Quaternion.LookRotation(transform.position);
    }
}
