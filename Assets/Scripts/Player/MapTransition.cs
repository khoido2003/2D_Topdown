using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D mapBoundary;

    [SerializeField]
    private CinemachineConfiner2D confiner2D;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private Direction direction;

    private void Awake() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner2D.BoundingShape2D = mapBoundary;

            MapUIController.Instance?.HighlightArea(mapBoundary.name);
        }
    }

    // Not needed this  ????
    private void UpdatePlayerPos(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += 2;
                break;

            case Direction.Down:
                newPos.y -= 2;
                break;

            case Direction.Left:
                newPos.x += 2;
                break;

            case Direction.Right:
                newPos.x -= 2;
                break;
        }

        player.transform.position = newPos;
    }
}
