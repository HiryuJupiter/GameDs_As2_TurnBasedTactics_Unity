using System.Collections;
using UnityEngine;

public class PlayerCardPlacement
{
    RealPlayer player;
    Camera camera;
    public PlayerCardPlacement(RealPlayer player)
    {
        this.player = player;
        camera = Camera.main;
    }

    public void TickUpdate ()
    {
        //Cancel card
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            player.CancelCardSelection();
        }

        //Raycast check for tile

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction  * 100f, Color.cyan);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            //Tile

            //Unit piece
        }
    }
}
