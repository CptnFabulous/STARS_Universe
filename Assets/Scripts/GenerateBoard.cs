using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour
{

    public Vector2 BoardScale;
    public Material DarkMaterial;
    public Material LightMaterial;


    private void Start()
    {

        bool whiteNotBlack = true;

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                Transform square = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
                square.parent = transform;
                square.localScale = BoardScale;
                square.position = new Vector2(x * BoardScale.x, y * BoardScale.y);

                if (whiteNotBlack)
                {
                    Debug.Log("White");
                    square.GetComponent<MeshRenderer>().material = LightMaterial;
                }
                else
                {
                    Debug.Log("Black");
                    square.GetComponent<MeshRenderer>().material = DarkMaterial;
                }

                whiteNotBlack = !whiteNotBlack;
            }

            whiteNotBlack = !whiteNotBlack;
        }
    }
}