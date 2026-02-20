using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public int   moveDistance = 10;
    public float gridSize = 1.0f;

    public bool moveHorinzontally = true;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    [SerializeField] private int direction = 1;
    private int numOfSteps = 0;
    private bool isMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        NextMovementTile();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition, 
                moveSpeed * Time.deltaTime
            );

            if(transform.position == targetPosition)
            {
                isMoving = false;
                numOfSteps++;

                if (numOfSteps >= moveDistance)
                {
                    direction *= -1;
                    numOfSteps = 0;
                }

                NextMovementTile();
            }

        }
    }

    void NextMovementTile()
    {
        Vector3 moveDirection;

        if (moveHorinzontally)
        {
            moveDirection = Vector3.right * direction;
        }
        else
        {
            moveDirection = Vector3.up * direction;
        }

        targetPosition = transform.position + moveDirection * gridSize;
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            PlayerSpawnManager.Instance.PlayerFellOff(collision.transform.GetChild(0).GetComponent<PlayerNetworkData>().Data.Value.PlayerID);
        }
    }
}
