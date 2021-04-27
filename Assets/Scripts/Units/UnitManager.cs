using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Layout;

namespace Units
{
    [DefaultExecutionOrder(-10)]
    public class UnitManager : MonoBehaviour
    {
        #region Variables
        public static UnitManager Instance;

        public Camera cam;
        public GameObject[] units = new GameObject[7];
        public GameObject dummyBot;
        public GameObject Zilla;
        public bool human;
        public Material highlight;
        public Material defaultTeam;
        public Hex currentHex;
        public bool selectMode;
        public bool spawnMode;
        public bool blueTurn;
        [SerializeField]
        private GameObject selectedUnit;
        private Hex selectedHex;

        PrefabDirectory dir;
        #endregion

        #region Start
        private void Awake()
        {
            Instance = this;
        dir = PrefabDirectory.Instance;
        }

        void Start()
        {
            blueTurn = true;
            spawnMode = true;
        }
        #endregion

        public BoardUnit SpawnUnit(CardTypes type, Hex hex, Player player)
        {
                
            BoardUnit unit = Instantiate(dir.GetUnitPiece(type), hex.attachPoint.position, hex.attachPoint.rotation) as BoardUnit;

           
            currentHex = hex;
            currentHex.attachedObject = unit.gameObject;
            unit.attachedHex = currentHex;
            unit.Initialize(player, type);
            //Vector3 pos = board.GetTileWorldPos(index.x, index.y);
            //Quaternion rot = player.IsMainPlayer ? P1Facing : P2Facing;
            //unit.SpawnInitialization(player, index);
            //AddUnitToList(unit, player.IsMainPlayer);
            return unit;
        }

        #region Spawn Mode
        //Where ive been trying to link - Ryan
        public void EnterSpawnMode(Card card)
        {
            StartCoroutine(SpawnMode(card));
        }
        IEnumerator SpawnMode(Card card)
        {
            spawnMode = true;

            while (spawnMode)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                //if (Physics.Raycast(ray, out hit))
                //{
                //    GameObject currentObj = hit.transform.gameObject;
                //    currentHex = currentObj.GetComponent<Hex>();

                //    if (currentHex.blue && currentHex.spawnable && currentHex.attachedObject == null)
                //    {
                //        currentHex.highlighted = true;

                //        if (Input.GetMouseButtonDown(0))
                //        {
                //            //Where ive been trying to link - Ryan
                //            GameObject newUnit = Instantiate(card.CardType, currentHex.attachPoint.position, currentHex.attachPoint.rotation);

                //            currentHex.attachedObject = newUnit;
                //            BoardUnit unitFunctions = newUnit.GetComponent<BoardUnit>();
                //            unitFunctions.attachedHex = currentHex;

                //            spawnMode = false;
                //            StartCoroutine(SelectMode());
                //            selectMode = true;
                //            yield return null;
                //        }
                //    }
                //}

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    spawnMode = false;
                    StartCoroutine(SelectMode());
                    selectMode = true;
                    yield return null;
                }
                yield return null;
            }
        }
        #endregion

        IEnumerator SelectMode()
        {
            selectMode = true;
            while (selectMode)
            {
                //Shoot Ray
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject currentObj = hit.transform.gameObject;
                    if (currentObj.CompareTag("Hex"))
                    {
                        currentHex = currentObj.GetComponent<Hex>();

                        currentHex.highlighted = true;

                        if (Input.GetMouseButtonDown(0) && currentHex.attachedObject != null)
                        {
                            selectedUnit = currentHex.gameObject;
                            Debug.Log(selectedUnit);
                            SelectUnit();
                        }

                        if (Input.GetMouseButtonDown(0) && selectedUnit != null)
                        {

                            if (currentHex.movable)
                            {
                                Hex newCurrentHex = selectedUnit.GetComponent<Hex>();
                                BoardUnit unit = newCurrentHex.attachedObject.GetComponent<BoardUnit>();
                                currentHex.attachedObject = unit.gameObject;
                                unit.ChangeToMove(currentHex);

                                DeselectUnit();
                            }
                            if (currentHex.attachedRed)
                            {
                                Hex newCurrentHex = selectedUnit.GetComponent<Hex>();
                                BoardUnit yourUnit = newCurrentHex.attachedObject.GetComponent<BoardUnit>();
                                BoardUnit enemyUnit = currentHex.attachedObject.GetComponent<BoardUnit>();
                                enemyUnit.currentHealth -= yourUnit.unitDamage;
                                Debug.Log(enemyUnit.currentHealth);
                                yourUnit.ChangeToAttack();

                                DeselectUnit();
                            }

                        }
                        /* if (Input.GetMouseButtonDown(0) && selectedUnit != null)
                         {
                             Hex newCurrentHex = selectedUnit.GetComponent<Hex>();
                             BoardUnit unit = newCurrentHex.attachedObject.GetComponent<BoardUnit>();
                             currentHex.attachedObject = unit.gameObject;
                             unit.ChangeToMove(currentHex);
                             DeselectUnit();
                         }
                         */
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DeselectUnit();
                    yield return null;
                }
                yield return null;
            }
        }

        public void SelectUnit()
        {
            
            currentHex.selected = true;
            selectedHex = currentHex;
        }
        public void DeselectUnit()
        {
            selectedHex.selected = false;
            selectedHex = null;
        }
        // Update is called once per frame
        void Update()
        {
            if (!blueTurn)
            {
                //Hex enemyHex = LayoutManager.hexPoints[7, Random.Range(0,5)].GetComponent<Hex>();
                //Instantiate(Zilla, enemyHex.attachPoint.position, enemyHex.attachPoint.rotation);
                //enemyHex.attachedObject = Zilla;
            }

            /* if (currentHex != null && currentHex.attachedObject != null)
             {

                 if (currentHex.gameObject == selectedUnit)
                 {

                     currentHex.selected = true;
                 }
                 else if (currentHex.gameObject != selectedUnit)
                 {
                     currentHex.selected = false;
                 }

             }
             */

            #region Dev Tools
#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectMode = false;
                StopCoroutine(SelectMode());
                //(SpawnMode(0));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectMode = false;
                StopCoroutine(SelectMode());
                //StartCoroutine(SpawnMode(1));
            }

            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                if (dummyBot != null)
                {
                    Hex enemyHex = LayoutManager.hexPoints[4, 4].GetComponent<Hex>();
                    Instantiate(dummyBot, enemyHex.attachPoint.position, enemyHex.attachPoint.rotation);
                    enemyHex.attachedObject = dummyBot;

                    BoardUnit dummyScript = dummyBot.GetComponent<BoardUnit>();
                    dummyScript.attachedHex = enemyHex;
                    dummyScript.dummy = true;
                }

            }


#endif
            #endregion
        }
    }
}