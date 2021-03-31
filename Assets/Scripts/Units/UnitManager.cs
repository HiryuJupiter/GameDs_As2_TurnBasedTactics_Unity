using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    
    public class UnitManager : MonoBehaviour
    {
        public float laneDistX = 0f;
        public float laneDistZ = 1.04f;
        public int horiLanes;
        public int[] vertLanes = new int[2];
        bool spawningBlue = true;
        
        public GameObject[] hex = new GameObject[2];
        public Transform spawnStart;
        
    

        // Start is called before the first frame update
        void Start()
        {
            spawningBlue = true;
            Transform[,] hexPoints = new Transform[vertLanes[0] + vertLanes[1], horiLanes];
            SpawnColour(0);
        }
      
        public void SpawnColour(int colour)
        {
            for (int i = 0; i < vertLanes[colour]; i++)
            {
                GameObject currentHex = Instantiate(hex[colour], spawnStart.position, spawnStart.rotation);

                spawnStart.position = new Vector3(spawnStart.position.x, spawnStart.position.y, spawnStart.position.z + laneDistZ);
               
            }

        }
        public void EndSpawning()
        {

        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

