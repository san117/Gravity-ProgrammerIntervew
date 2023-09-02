using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    #region Fields
    [SerializeField] private Vector2 _destinyPosition;
    [SerializeField] private UnityEvent _onUseDoor;
    #endregion

    #region Unity
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.transform.root.tag == "Player")
        {
            PlayerController.Singleton.transform.position = _destinyPosition;
            _onUseDoor.Invoke();
        }
    }
    #endregion

}
