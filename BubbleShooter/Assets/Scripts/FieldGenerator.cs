using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//висит на гейм-объекте Fields
public class FieldGenerator : MonoBehaviour
{
    #region Variables

    public GameObject ballFreezed;

    private Vector3 startPosition = new Vector3(0, 0, 0);
    private float ballsXIndexInRow = 0;
    private int rowY = 0;

    #endregion Variables

    void Start()
    {
        GenerateFields(4); //создать четыре ряда бесцветных(белых) шаров (раскрашивание в классе Shooting)
        ColorizeBalls();
    }

    #region Methods

    private void GenerateFields(float rowsCount)
    {
        bool flag = true;
       // rowsCount = rowsCount;

        for (int i = 0; i < rowsCount; i++)
        {
            if (flag == true)
            {
                GenerateFieldsEight();
                flag = false;
            }

            else if (flag == false)
            {
                GenerateFieldsSeven();
                flag = true;
            }
        }
    }

    private void GenerateFieldsSeven()
    {
        for (float i = ballsXIndexInRow; i < 7; i++)
        {
            InitField(i);

            if (startPosition.x == 6.5f)
            {
                startPosition.x = 0f;
                startPosition.y = startPosition.y - 1f;
                ballsXIndexInRow = 0f;
                rowY++;
            }
        }
    }

    private void GenerateFieldsEight()
    {
        for (float i = ballsXIndexInRow; i < 8; i++)
        {
            InitField(i);

            if (startPosition.x == 7)
            {
                startPosition.x = 0.5f;
                startPosition.y = startPosition.y - 1f;
                ballsXIndexInRow = 0.5f;
                rowY++;
            }
        }
    }

    private void InitField(float i)
    {
        startPosition.x = i;
        GameObject field = (GameObject)Instantiate(ballFreezed, startPosition, Quaternion.identity);
        field.transform.SetParent(this.gameObject.transform);
        field.transform.localPosition = startPosition;
        field.name = rowY + "x" + startPosition.x;
    }

    private void ColorizeBalls()
    {
        IEnumerable<Transform> children = GameObject.Find("Fields").GetComponentsInChildren<Transform>();

        foreach (var go in children)
        {
            if (go.tag == "Base")
            {
                //1-й ряд шаров
                if (go.name == "0x0" || go.name == "0x1")
                {
                    go.GetComponent<Renderer>().material.color = Color.grey;
                }
                else if (go.name == "0x2" || go.name == "0x3")
                {
                    go.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (go.name == "0x4" || go.name == "0x5")
                {
                    go.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (go.name == "0x6" || go.name == "0x7")
                {
                    go.GetComponent<Renderer>().material.color = Color.green;
                }

                //2-й ряд шаров
                if (go.name == "1x0.5" || go.name == "1x1.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.grey;
                }
                else if (go.name == "1x2.5" || go.name == "1x3.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (go.name == "1x4.5" || go.name == "1x5.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (go.name == "1x6.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.green;
                }

                //3-й ряд шаров
                if (go.name == "2x0" || go.name == "2x1")
                {
                    go.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (go.name == "2x2" || go.name == "2x3")
                {
                    go.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (go.name == "2x4" || go.name == "2x5")
                {
                    go.GetComponent<Renderer>().material.color = Color.grey;
                }
                else if (go.name == "2x6" || go.name == "2x7")
                {
                    go.GetComponent<Renderer>().material.color = Color.red;
                }

                //4-ряд
                if (go.name == "3x0.5" || go.name == "3x1.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (go.name == "3x2.5" || go.name == "3x3.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (go.name == "3x4.5" || go.name == "3x5.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.grey;
                }
                else if (go.name == "3x6.5" || go.name == "3x7.5")
                {
                    go.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    #endregion Methods
}
