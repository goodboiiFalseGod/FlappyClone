using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initilizer : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private ColumnSpawner _columnSpawner;
    [SerializeField] private PlayerMovement _player;

    static Dictionary<Corner, Vector2> corners;

    public void Start()
    {
        _columnSpawner.Init(_gameSettings);
        
    }

    public static Vector2 GetCornerPosition(Corner corner)
    {
        if (corners == null)
        {
            var upperLeftScreen = new Vector3(0, Screen.height, 0);
            var upperRightScreen = new Vector3(Screen.width, Screen.height, 0);
            var lowerLeftScreen = new Vector3(0, 0, 0);
            var lowerRightScreen = new Vector3(Screen.width, 0, 0);

            //Corner locations in world coordinates
            Vector2 upperLeft = Camera.main.ScreenToWorldPoint(upperLeftScreen);
            Vector2 upperRight = Camera.main.ScreenToWorldPoint(upperRightScreen);
            Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(lowerLeftScreen);
            Vector2 lowerRight = Camera.main.ScreenToWorldPoint(lowerRightScreen);

            corners = new Dictionary<Corner, Vector2>();
            corners.Add(Corner.LeftUpper, upperLeft);
            corners.Add(Corner.LeftLower, lowerLeft);
            corners.Add(Corner.RightUpper, upperRight);
            corners.Add(Corner.RightLower, lowerRight);
        }

        return corners[corner];
    }
}
