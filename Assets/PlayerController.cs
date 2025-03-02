using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private float playerSpeed = 10f;
    
    private Camera _testCamera;
    private Coroutine _coroutine;
    private Vector3 targetposition;

    private void Awake()
    {
        _testCamera = Camera.main;
    }

    private void OnEnable()
    {
       MouseClick.Enable();
       MouseClick.performed += Move;
       
    }

    private void OnDisable()
    {
        MouseClick.performed -= Move;
        MouseClick.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Ray ray = _testCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
           _coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
           targetposition = hit.point;
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position,target,playerSpeed * Time.deltaTime);
            transform.position = destination;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetposition,1);
    }
}
