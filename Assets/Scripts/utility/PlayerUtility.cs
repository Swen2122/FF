using UnityEngine;

public class PlayerUtility : MonoBehaviour
{
    private static Transform _playerTransform;

    public static Transform PlayerTransform
    {
        get
        {
            if (_playerTransform == null)
            {
                GameObject playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    _playerTransform = playerObject.transform;
                }
                else
                {
                    Debug.LogError("ќб'Їкт з тегом 'Player' не знайдено!");
                }
            }
            return _playerTransform;
        }
    }
}
