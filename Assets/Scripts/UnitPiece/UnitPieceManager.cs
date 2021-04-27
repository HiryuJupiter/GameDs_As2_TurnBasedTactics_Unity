using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitPieceManager : MonoBehaviour
{
    public static UnitPieceManager Instance;

    BoardManager board;
    PrefabDirectory dir;
    

    Quaternion P1Facing, P2Facing;

    public List<PlayerUnit> P1Pieces {get; private set;} = new List<PlayerUnit>();
    public List<PlayerUnit> P2Pieces { get; private set; } = new List<PlayerUnit>();

    private void Awake()
    {
        Instance = this;
        P1Facing = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        P2Facing = Quaternion.LookRotation(-Vector3.forward, Vector3.up);
    }

    private void Start()
    {
        board = BoardManager.Instance;
        dir = PrefabDirectory.Instance;
    }

    private void Update()
    {

    }

    public PlayerUnit SpawnUnit (CardTypes type, Vector2Int index, Player player)
    {
        Vector3 pos = board.GetTileWorldPos(index.x, index.y);
        Quaternion rot = player.IsMainPlayer ? P1Facing : P2Facing;
        PlayerUnit unit = Instantiate(dir.GetUnitPiece(type), pos, rot) as PlayerUnit;
        unit.SpawnInitialization(player, index);

        AddUnitToList(unit, player.IsMainPlayer);
        return unit;
    }

    #region Minor
    void AddUnitToList (PlayerUnit unit, bool isMainPlayer )
    {
        (isMainPlayer ? P1Pieces : P2Pieces).Add(unit);
    }
    #endregion
}