using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDetector : MonoBehaviour
{
    private Vector2 movementInput;
    public Vector2 MovmentInput
    { get { return movementInput; } }

    private PlayerInputs playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputs();
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = playerInput.BasicMovement.WASD.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

}
