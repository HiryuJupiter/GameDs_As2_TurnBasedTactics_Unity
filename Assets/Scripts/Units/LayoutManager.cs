using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Layout
{
    
    public class LayoutManager : MonoBehaviour
    {
        [Header("Offsets")]
        public float laneDistX = 0f;
        public float laneDistZ = 1.56f;
        public float offsetDist;

        [Header("Map Size")]
        public int horiLanes;
        public int[] vertLanes = new int[2];
        bool spawningBlue = true;
        bool oddLane;
        int colour;
        int currentLaneSpawned;
        public float startoffsetX;
        public float startoffsetY;
        public float startoffsetZ;
        Vector3 spawnStartOffset;
        public static GameObject[,] hexPoints;


        public GameObject[] hex = new GameObject[2];
        public Transform spawnStart;
        
    

        // Start is called before the first frame update
        void Start()
        {
            spawnStartOffset = new Vector3(startoffsetX, startoffsetY, startoffsetZ);
            spawnStart.position = spawnStartOffset;
            spawningBlue = true;
            oddLane = true;
            currentLaneSpawned = 0;
            hexPoints = new GameObject[vertLanes[0] + vertLanes[1], horiLanes];
            SpawnColour();
        }
      
        public void SpawnColour()
        {
            
            if(spawningBlue == true)
            {
                colour = 0;
            }
            else
            {
                colour = 1;
            }
            for (int i = 0; i < vertLanes[colour]; i++)
            {

                GameObject currentHex = Instantiate(hex[colour], spawnStart.position, spawnStart.rotation);
                int xPoint = i + colour * vertLanes[colour];
                hexPoints[xPoint, currentLaneSpawned] = currentHex;
                Hex newHex = currentHex.GetComponent<Hex>();
                newHex.vertPoint = xPoint;
                newHex.horiPoint = currentLaneSpawned;
                newHex.oddLane = oddLane;

                if (i == 0)
                {
                   
                    newHex.spawnable = true;
                }

                spawnStart.position = new Vector3(spawnStart.position.x, spawnStart.position.y, spawnStart.position.z + laneDistZ);

            }

            if (!spawningBlue)
            {
                currentLaneSpawned++;
                if (oddLane)
                {
                    spawnStart.position = new Vector3(1.35f * currentLaneSpawned, 0, -0.78f) + spawnStartOffset;
                }
                else
                {
                    spawnStart.position = new Vector3(1.35f * currentLaneSpawned, 0, 0) + spawnStartOffset;
                }
                // Change from odd lane to even
                oddLane = !oddLane;
            }
           
            // Change hexagon colour
            spawningBlue = !spawningBlue;

            
            if (currentLaneSpawned < horiLanes)
            {
                SpawnColour();
            }
            

        }
       
    }
}

