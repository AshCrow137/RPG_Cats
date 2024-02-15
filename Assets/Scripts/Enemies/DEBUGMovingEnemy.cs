using UnityEngine;

public class DEBUGMovingEnemy : EnemyScript
{

    public float moveDistance = 10;
    public float speed = 5;

    private int Direction = 1;
    private float fMinX;
    private float fMaxX;

    private void Start()
    {
        fMinX = transform.position.x - moveDistance;
        fMaxX = transform.position.x + moveDistance;
    }

    void Update()
    {
        float fEnemyX = transform.position.x;
        switch (Direction)
        {
            case -1:
                // Moving Left
                if (fEnemyX > fMinX)
                {
                    fEnemyX -= speed * Time.deltaTime;
                }
                else
                {
                    // Hit left boundary, change direction
                    Direction = 1;
                }
                break;

            case 1:
                // Moving Right
                if (fEnemyX < fMaxX)
                {
                    fEnemyX += speed * Time.deltaTime;
                }
                else
                {
                    // Hit right boundary, change direction
                    Direction = -1;
                }
                break;
        }

        transform.position = new Vector3(fEnemyX, transform.position.y, transform.position.z);
    }
}
