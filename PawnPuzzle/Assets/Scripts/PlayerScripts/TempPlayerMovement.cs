using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayerMovement : NetworkBehaviour
{
    [SerializeField] Transform playerTransform;

    private PlayerInputs controls;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float gridSize = 1.0f;

    private bool isMoving = false;

    void Awake()
    {
        controls = new PlayerInputs();

        controls.BasicMovement.WASD.performed += OnMoveInput;
    }

    public override void OnNetworkSpawn()
    {
        // Only owner moves
        if (!IsOwner)
        {
            Destroy(this);
            return;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (isMoving) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            StartCoroutine(Move(input));
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        //Becuase of the nature of our levels, I am adding a special condition here
        if(Mathf.Abs(direction.y) == 1)
        {
            gridSize = 1.15f;
        }
        else
        {
            gridSize = 1.5f;
        }

            isMoving = true;

        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;

            playerTransform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        playerTransform.position = endPosition;

        isMoving = false;
    }
}
