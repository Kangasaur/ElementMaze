using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    private CharacterController charController;
    private float speed = 5;
    private float mouseSensitivity = 3.5f;

    Transform cameraTrans;
    float cameraPitch = 0;
    float gravityValue = Physics.gravity.y;
    float jumpHeight = -2f;

    float currentYVelocity;

    [SerializeField] TextMeshProUGUI orbText;
    [SerializeField] float throwForce = 10f;
    [SerializeField] PrefabDatabase prefabDB;
    SphereCollider trigger;


    Stack<int> orbStack = new Stack<int>();
    string[] orbTypes = new string[] { "Fire", "Water", "Earth" };



    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        cameraTrans = Camera.main.transform;
        trigger = GetComponent<SphereCollider>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);

        //Constraint the camera pitch inbetween -90 to 90
        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
        cameraTrans.localEulerAngles = Vector3.right * cameraPitch;
        //cameraTrans.Rotate(Vector3.left * mouseDelta.y * mouseSensitivity);

        if (Input.GetMouseButtonDown(0) && orbStack.Count > 0) ThrowOrb();

        Vector3 move = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (charController.isGrounded && currentYVelocity < 0)
        {
            currentYVelocity = 0f;
        }

        move.y = currentYVelocity;
        currentYVelocity += gravityValue * Time.deltaTime;

        charController.Move(move * Time.deltaTime * speed);
    }

    public void AddOrb(string orbType)
    {
        orbStack.Push(Array.IndexOf(orbTypes, orbType));
        orbText.text = "Top orb in backpack: " + orbType;
    }

    public void ThrowOrb()
    {
        int orbType = orbStack.Pop();
        trigger.enabled = false;
        Vector3 throwDirection = cameraTrans.TransformDirection(Vector3.forward);
        GameObject newOrb = Instantiate(prefabDB.orbPrefabs[orbType], transform.position + throwDirection, transform.rotation);
        newOrb.GetComponent<Rigidbody>().velocity = throwDirection * throwForce;
        orbText.text = orbStack.Count > 0 ? "Top orb in backpack: " + orbTypes[orbStack.Peek()] : "Backpack is empty";
        Invoke("ReenableTrigger", 0.2f);
    }

    void ReenableTrigger()
    {
        trigger.enabled = true;
    }

	/*private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
	}*/
}
