// Name: BillboardUI.cs
// Author: Connor Larsen
// Date 02/05/2022

using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] Transform cameraTransform; // Reference to the transform of the player camera
    #endregion

    #region Private Variables
    Quaternion startRot;    // Starting rotation of the UI element
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;  // Set the start rotation on script startup
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cameraTransform.rotation * startRot;   // UI element faces the player
    }
    #endregion
}