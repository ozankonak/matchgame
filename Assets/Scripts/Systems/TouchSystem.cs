using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSystem : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero,layer);

            if (hit.collider != null && !hit.collider.GetComponent<Tile>().clicked)
            {
                hit.collider.GetComponent<Tile>().clicked = true;
                EventManager.TriggerEvent(EventManager.tileClickedEvent, hit.collider.gameObject);
            }
        }
    }

}
