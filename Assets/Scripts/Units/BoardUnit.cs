using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Layout;

public class BoardUnit : MonoBehaviour
{
    #region Variables
    [Header("Unit")]
    public int unitType;
    public string unitName;
    public int maxHealth;
    public int currentHealth;
    public int unitDamage;
    public bool blue = true;
    public bool selected;
    public Hex attachedHex;
    private List<GameObject> movableHexes = new List<GameObject>();
    private float transferSpeed;
    private float transferSpeedMulti = 5;
    public bool dummy;

    [Header("UI")]
    public Text unitNameText;
    public Text currentHealthText;
    public Text unitDamageText;

    PlayerUnitOldSchool unitOld;
    public bool IsMainPlayer => true;

    GameSettings settigns;
    #endregion
    #region Enum
    public enum unitState
    {
        
        Idle,
        SelectingPosition,
        Moving,
        Attacking,
    }
    public unitState state;
    #endregion
    #region Start
    //Where ive been trying to link - Ryan
    public void Initialize(Player player, CardTypes card)
    {
        switch (card)
        {
            case CardTypes.Godzilla:
                unitName = "King";
                maxHealth = 200;
                unitDamage = 20;
                break;
            case CardTypes.Jack:
                unitName = "Jack";
                maxHealth = 200;
                unitDamage = 20;
                break;
            case CardTypes.Swordsman:
                unitName = "Swordsman";
                maxHealth = 100;
                unitDamage = 10;
                break;
            case CardTypes.Spearman:
                unitName = "Spearman";
                maxHealth = 100;
                unitDamage = 20;
                break;
            default:
                unitName = "Swordsman";
                maxHealth = 100;
                unitDamage = 10;
                break;


        }

        settigns = GameSettings.Instance;

        transferSpeed = transferSpeedMulti * Time.deltaTime;
        unitOld = GetComponent<PlayerUnitOldSchool>();
        RefreshStats();

        unitOld.SpawnInitialization(player);
    }
    void Start()
    {
        
        
        //if (!dummy)
        //{
        //    StartCoroutine(Idle());
        //}


    }
    #endregion
    #region Update
    void Update()
    {
        if(currentHealth == 0)
        {
            Death();
        }
    }
    #endregion
    #region State Machine
    IEnumerator Idle()
    {
        #region Idle
        while (state == unitState.Idle)
        {
            if (attachedHex.selected == true)
            {
                Debug.Log("Selecting Position " + unitName);
                state = unitState.SelectingPosition;
                
            }
            yield return 0;
        }
        #endregion
        #region Selecting
        while (state == unitState.SelectingPosition)
        {
            int currentX = attachedHex.horiPoint;
            int currentY = attachedHex.vertPoint;

            if(attachedHex.oddLane == true)
            {
                if(currentY < 7)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX]);
                    if(currentX < 5)
                    {
                        movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX + 1]);
                    }
                    if (currentX > 0)
                    {
                        movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX - 1]);
                    }
                }
                
                if(currentY > 0)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX]);
                }
                if(currentX > 0)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX - 1]);
                }
                if(currentX < 5)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX + 1]);
                }
                
                
            }
            else
            {
                if (currentY > 0)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX]);


                    if (currentX > 0)
                    {
                        movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX - 1]);
                    }
                }
                if(currentX > 0)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX - 1]);
                }

                if (currentY < 7)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX]);

                   
                }
                if (currentX < 5)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX + 1]);
                    if (currentY > 0)
                    {
                        movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX + 1]);
                    }
                }
            }


            foreach(GameObject highlighedHex in movableHexes)
            {
                
                Hex newHex = highlighedHex.GetComponent<Hex>();
                if(newHex.attachedObject != null)
                {
                    newHex.movable = false;

                    if (newHex.attachedObject.GetComponent<BoardUnit>().blue == false)
                    {
                        newHex.movable = false;
                        newHex.attachedRed = true;
                    }
                }
                else
                {
                    newHex.movable = true;
                }
            }
             
            yield return 0;
        }
        #endregion
        #region Moving
        while (state == unitState.Moving)
        {
            Debug.Log("Moving" + unitName);

            //Moving Towards new Hexagon
            if(this.gameObject.transform.position != attachedHex.attachPoint.transform.position)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, attachedHex.attachPoint.transform.position, transferSpeed);

                foreach(GameObject oldHex in movableHexes)
                {
                    Hex hex = oldHex.GetComponent<Hex>();
                    hex.movable = false;

                }
            }
            if(this.gameObject.transform.position == attachedHex.attachPoint.transform.position)
            {
                movableHexes.Clear();
                state = unitState.Idle;
                StartCoroutine(Idle());
            }
            yield return 0;
        }
        #endregion
        #region Attacking
        while (state == unitState.Attacking)
        {
            foreach (GameObject oldHex in movableHexes)
            {
                Hex hex = oldHex.GetComponent<Hex>();
                hex.movable = false;

            }
            movableHexes.Clear();
            state = unitState.Idle;
            StartCoroutine(Idle());

            yield return 0;
        }
        yield return null;
        #endregion
    }
    #endregion
 
    public void MoveToPosition (Vector3 tgt)
    {
        StartCoroutine(DoMove(tgt));
    }

    IEnumerator DoMove (Vector3 tgt)
    {
        float t = 0;
        Vector3 start = transform.position;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            transform.position = Vector3.Lerp(start, tgt, t);
            yield return null;
        }
    }

    public void ChangeToMove(Hex newHex)
    {
       attachedHex.attachedObject = null;
       attachedHex = newHex;
        
       state = unitState.Moving;
    }
    public void ChangeToAttack()
    {
        state = unitState.Attacking;
    }
    public void Death()
    {
        this.gameObject.SetActive(false);
    }

    public void RefreshStats()
    {
        currentHealth = maxHealth;
        if (unitNameText != null)
        {
            unitNameText.text = "Unit: " + unitName;
            currentHealthText.text = "Health: " + currentHealth;
            unitDamageText.text = "Damage: " + unitDamage;
        }
    }

    public void ToggleHoverHighlight(bool isOn)
    {
        unitOld.TogglehoverHighlight(isOn);
    }

    public void ToggleSelectionHighlight (bool isOn)
    {
        unitOld.ToggleSelectionHighlight(isOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (settigns.IsOnEnemyLayer(other))
        {
            GameState.Instance.EarnGold(150);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("other " + other.gameObject.name);
        }
    }
}
