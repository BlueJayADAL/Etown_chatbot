using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerPosition : MonoBehaviour
{
    public Text positionText; // Reference to the Text component to display the position
    public Transform targetObject; // Reference to the object whose position will be displayed

    private void Update()
    {
        if (targetObject != null)
        {
            // Update the position text with the current object position
            positionText.text = "Position: " + targetObject.position.ToString("F2");
        }
    }
}