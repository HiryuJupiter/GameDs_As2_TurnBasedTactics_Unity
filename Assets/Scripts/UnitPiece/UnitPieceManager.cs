using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPieceManager : MonoBehaviour
{
    public static UnitPieceManager Instance;

    BoardManager board;
    PrefabDirectory dir;

    Quaternion P1Facing, P2Facing;

    public List<UnitPiece> P1Pieces {get; private set;} = new List<UnitPiece>();
    public List<UnitPiece> P2Pieces { get; private set; } = new List<UnitPiece>();

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

    public UnitPiece SpawnUnit (CardTypes type, Vector2Int index, Player player)
    {
        Vector3 pos = board.GetTileWorldPos(index.x, index.y);
        Quaternion rot = player.IsMainPlayer ? P1Facing : P2Facing;
        UnitPiece unit = Instantiate(dir.GetUnitPiece(type), pos, rot) as UnitPiece;
        unit.SpawnInitialization(player, index);

        AddUnitToList(unit, player.IsMainPlayer);
        return unit;
    }

    #region Minor
    void AddUnitToList (UnitPiece unit, bool isMainPlayer )
    {
        (isMainPlayer ? P1Pieces : P2Pieces).Add(unit);
    }
    #endregion
}