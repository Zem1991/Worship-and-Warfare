using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Singleton;

    [Header("Prefabs")]
    public Piece prefabPiece;

    [Header("Instances")]
    public List<Piece> pieces;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of PieceManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeletePieces()
    {
        foreach (var item in pieces)
        {
            Destroy(item);
        }
        pieces.Clear();
    }

    public void CreatePieces(PieceData[] data)
    {
        DeletePieces();

        pieces = new List<Piece>();
        foreach (var item in data)
        {
            int posX = item.mapPosition[0];
            int posZ = item.mapPosition[1];

            Vector3 pos = new Vector3(posX, 0, posZ);
            Quaternion rot = Quaternion.identity;

            Piece newPiece = Instantiate(prefabPiece, pos, rot, transform);
            pieces.Add(newPiece);

            newPiece.mapPosition = new Vector2(posX, posZ);
            newPiece.name = item.name;
        }
    }
}
