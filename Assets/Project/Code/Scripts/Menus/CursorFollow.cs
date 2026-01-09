using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorFollow : MonoBehaviour
{
    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private List<RectTransform> buttonList;
    [SerializeField] private Image lineInteraction;

    private void Update()
    {
        Vector2 newPosition;
        newPosition = Vector2.MoveTowards(transform.position, mousePosition.action.ReadValue<Vector2>(), followSpeed);
        newPosition.x = transform.position.x;

        transform.position = newPosition;

        bool interactionDetected = false;

        foreach (RectTransform t in buttonList)
        {
            Vector2 dir = (Vector2)(t.position - transform.position);
            Vector2 mouseDir = (Vector2)(mousePosition.action.ReadValue<Vector2>() - (Vector2)transform.position);
            float absoluteDot = Mathf.Abs(Vector2.Dot(transform.up, dir.normalized));
            float absoluteDotMouse = Mathf.Abs(Vector2.Dot(transform.up, mouseDir.normalized));
            Debug.DrawLine(transform.position, t.position);

            if (absoluteDot < 0.025f && absoluteDotMouse < 0.05f)
            {
                interactionDetected = true;
                break;
            }
        }

        lineInteraction.gameObject.SetActive(interactionDetected);
    }
}
