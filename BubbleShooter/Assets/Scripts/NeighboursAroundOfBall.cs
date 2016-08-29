using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NeighboursAroundOfBall : MonoBehaviour
{
    string topLeftName = null;
    string topRightName = null;
    string bottomLeftName = null;
    string bottomRightName = null;
    string leftName = null;
    string rightName = null;

    //ссылки на шаров-соседей
    [HideInInspector]
    public GameObject TopLeft = null;
    
    [HideInInspector]
    public GameObject TopRight = null;

    [HideInInspector]
    public GameObject Left = null;

    [HideInInspector]
    public GameObject Right = null;

    [HideInInspector]
    public GameObject BottomLeft = null;

    [HideInInspector]
    public GameObject BottomRight = null;

    //список соседей-шаров определнного шара
    [HideInInspector]
    public List<GameObject> listOfGameObjects = new List<GameObject>();

    void Start()
    {
        InitNeighboursOfBall();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            InitNeighboursOfBall();
    }
        
    void OnCollisionEnter(Collision col)
    {
        InitNeighboursOfBall();
    }

    //ДЛЯ КАЖДОГО ШАРА ПО ЕГО ПЕРИМЕТРУ ЕСТЬ МАКСИМУМ 6 ШАРОВ-СОСЕДЕЙ
    //каждый шар хранит ссылки на них
    private void InitNeighboursOfBall()
    {
        float x = GetComponent<Transform>().localPosition.x;
        float y = GetComponent<Transform>().localPosition.y;

        if (y < 0)
        {
            if (x > 0)
            {
                topLeftName = string.Format("{0}x{1}", Mathf.Abs(y + 1f), x - 0.5f);
                TopLeft = GameObject.Find(topLeftName);
                if (TopLeft) listOfGameObjects.Add(TopLeft);
            }
            if (x < 7)
            {
                topRightName = string.Format("{0}x{1}", Mathf.Abs(y + 1f), x + 0.5f);
                TopRight = GameObject.Find(topRightName);
                if (TopRight) listOfGameObjects.Add(TopRight);
            }
        }
        if (x > 0)
        {
            bottomLeftName = string.Format("{0}x{1}", Mathf.Abs(y - 1f), x - 0.5f);
            BottomLeft = GameObject.Find(bottomLeftName);
            if (BottomLeft) listOfGameObjects.Add(BottomLeft);
        }
        if (x < 7)
        {
            bottomRightName = string.Format("{0}x{1}", Mathf.Abs(y - 1f), x + 0.5f);
            BottomRight = GameObject.Find(bottomRightName);
            if (BottomRight) listOfGameObjects.Add(BottomRight);
        }
        // для крайних шариков нету боковых соседей
        if (x != 6.5f && x != 7)
        {
            rightName = string.Format("{0}x{1}", Mathf.Abs(y), x + 1);
            Right = GameObject.Find(rightName);
            if (Right) listOfGameObjects.Add(Right);
        }
        if (x != 0f && x != 0.5)
        {
            leftName = string.Format("{0}x{1}", Mathf.Abs(y), x - 1);
            Left = GameObject.Find(leftName);
            if (Left) listOfGameObjects.Add(Left);
        }
    }


}
