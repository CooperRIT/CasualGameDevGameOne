using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridMovement : MonoBehaviour
{

    private PlayerInputs controls;
    [SerializeField] private bool isRepeatable = false;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float gridSize = 1.0f;

    private bool isMoving = false;


    private void Awake()
    {
        //will comment later
        controls = new PlayerInputs();

        controls.BasicMovement.WASD.performed += OnMoveInput;

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (isMoving) return;

        Vector2 input = context.ReadValue<Vector2>();

        if(Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
            input.y = 0;
        }
        else
        {
            input.x = 0;
        }

        if(input != Vector2.zero)
        {
            StartCoroutine(Move(input));
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        isMoving = true;

        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        float elapsedTime = 0;

        while(elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;

            transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        transform.position = endPosition;

        isMoving = false;
    }
}
