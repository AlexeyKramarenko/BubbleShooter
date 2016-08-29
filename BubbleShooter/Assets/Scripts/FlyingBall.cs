using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
public class FlyingBall : MonoBehaviour
{
    #region Variables

    public static float lastElY = 0;
    //поиск шаров одиночек, которые остались после удалении серии бинго
    public static bool startFindSingletons = false;

    #endregion Variables

    void Start()
    {
        lastElY = GetLastElemY();
    }

    void Update()
    {
        lastElY = GetLastElemY();

        if (startFindSingletons)
        {
            FindAndDeleteSingletons();
            startFindSingletons = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //ШАР ПОПАЛ ПО СТЕНЕ
        if (col.gameObject.tag == "Wall")
        {
            Bounce(col);
        }
        //ШАР ПОПАЛ ПО ПОТОЛКУ
        else if (col.gameObject.tag == "Ceiling")
        {
            Shooting.shooting = false;
            Destroy(this.gameObject);
        }
        //ШАР ПОПАЛ В ДРУГОЙ ШАР
        else
        {
            FreezeBall();
            StartCoroutine(Bingo(col));
        }
    }


    #region Methods
    
    private IEnumerator Bingo(Collision col)
    {
        AttachToTarget(col);

        yield return new WaitForSeconds(0.3f);
        List<GameObject> objs = this.GetComponent<NeighboursAroundOfBall>().listOfGameObjects;

        //ЛУКОВИЦА 1 cлой
        List<GameObject> list1 = new List<GameObject>(); //одного цвета
        if (objs.Any())
            foreach (var t in objs)
            {
                if (t.gameObject != null && t.GetComponent<Renderer>().material.color == gameObject.GetComponent<Renderer>().material.color && !list1.Contains(t))

                    list1.Add(t);

            }

        var result = GetNeighboursBallsTheSameColor();

        if (result.Count > 2)
        {
            RemoveBallsAndReferencesOnThem(result);

            startFindSingletons = true;

            yield return new WaitForSeconds(0.5f);

            if (gameObject != null) Destroy(this.gameObject); //чтобы не было MissingReferenceException


        }
        Shooting.shooting = false;

        //Удаление этого скрипта обязательно для шаров после их контакта с верхними неподвижными
        //иначе в будущем при попадании в такой шар они будут рикошетить друг от друга (тот, что висит и который летит в него)
        Destroy(this.GetComponent("FlyingBall"));

    }

    void FindNeighboursAroundCurrentList(List<GameObject> currentList, List<GameObject> neighboursList)
    {
        if (currentList.Any())
            foreach (var t in currentList)
            {
                List<GameObject> T = t.GetComponent<NeighboursAroundOfBall>().listOfGameObjects;
                foreach (var u in T)
                {
                    if (u.gameObject != null && u.GetComponent<Renderer>().material.color == gameObject.GetComponent<Renderer>().material.color && !neighboursList.Contains(u))
                    {
                        neighboursList.Add(u);
                    }
                }
            }
    }
    
    void RemoveBallsAndReferencesOnThem(List<GameObject> result)
    {
        //получили список обьектов 
        //перед их удалением удаляем все ссылки на них у других шаров
        Transform[] objects = GameObject.Find("Fields").GetComponentsInChildren<Transform>();

        for (int i = 0; i < result.Count; i++)
        {
            for (int j = 1; j < objects.Length; j++)
            {
                if (objects[j])
                {
                    if (objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().TopLeft == result[i])
                    {
                        objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().TopLeft = null;
                    }
                    if (objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().TopRight == result[i])
                    {
                        objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().TopRight = null;
                    }
                    if (objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().Left == result[i])
                    {
                        objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().Left = null;
                    }
                    if (objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().Right == result[i])
                    {
                        objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().Right = null;
                    }
                    if (objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().BottomLeft == result[i])
                    {
                        objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().BottomLeft = null;
                    }
                    if (objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().BottomRight == result[i])
                    {
                        objects[j].gameObject.GetComponent<NeighboursAroundOfBall>().BottomRight = null;
                    }
                }
            }
        }
        Shooting.shooting = false;


        foreach (var a in result)
        {
            if (a.gameObject != null)
            {
                Destroy(a.gameObject);
            }
        }
    }

    List<GameObject> GetNeighboursBallsTheSameColor()
    {
        List<GameObject> listOfGameObjects = this.GetComponent<NeighboursAroundOfBall>().listOfGameObjects;

        List<List<GameObject>> generalList = new List<List<GameObject>>();

        //резервируем 7 слоев-списков для поиска соседей шарика, на котором висит этот скрипт (слой - это шары вокруг (по периметру))
        for (int k = 0; k < 7; k++)
        {
            generalList.Add(new List<GameObject>());
        }

        //НАХОДИМ ПЕРВЫЙ СЛОЙ ШАРИКОВ ТОГО ЖЕ ЦВЕТА ЧТО И ТОТ ШАР, НА КОТОРОМ ВИСИТ ЭТОТ СКРИПТ
        if (listOfGameObjects.Any())
            foreach (var t in listOfGameObjects)
            {
                if (t.gameObject != null && t.GetComponent<Renderer>().material.color == gameObject.GetComponent<Renderer>().material.color && !generalList[0].Contains(t))

                    generalList[0].Add(t);
            }

        //ДЛЯ КАЖДОГО ШАРА ТОГО ЖЕ ЦВЕТА НАХОДИМ АНАЛОГИЧНЫЕ СЛОЙ ЗА СЛОЕМ 
        for (int i = 0; i < generalList.Count - 1; i++)
        {
            FindNeighboursAroundCurrentList(generalList[i], generalList[i + 1]);
        }

        //СПИСОК ШАРОВ ОДНОГО ЦВЕТА и без повторений вокру шара, на котором висит этот скрипт
        List<GameObject> resultList = new List<GameObject>();

        if (!resultList.Contains(gameObject))
            resultList.Add(this.gameObject);

        foreach (List<GameObject> list in generalList)
        {
            foreach (GameObject go in list)
            {
                if (!resultList.Contains(go))
                    resultList.Add(go);
            }
        }

        return resultList;
    }

    private void AttachToTarget(Collision col)
    {
        Vector3 enemyPos = col.gameObject.GetComponent<Transform>().localPosition;
        NeighboursAroundOfBall enemySide = col.gameObject.GetComponent<NeighboursAroundOfBall>();

        float ballDiameter = transform.localScale.x;

        //значения DELTA
        float X = this.transform.localPosition.x - col.transform.localPosition.x;
        float Y = this.transform.localPosition.y - col.transform.localPosition.y;

        //ЛИПНЕМ В ПРАВУЮ НИЖНЮЮ ЧАСТЬ
        if (X >= 0 && X < 0.7f * ballDiameter && Y <= 0.5
            && enemySide.BottomRight == null)
        {
            this.transform.localPosition = new Vector3(enemyPos.x + 0.5f, enemyPos.y - 1, 0);
            this.name = (Math.Abs(enemyPos.y - 1)).ToString() + "x" + (enemyPos.x + 0.5f);

            //ДЛЯ КРАЙНИХ ШАРОВ
            if (enemyPos.x == 7)
            {
                this.transform.localPosition = new Vector3(enemyPos.x - 0.5f, enemyPos.y - 1, 0);
                this.name = (Math.Abs(enemyPos.y - 1)).ToString() + "x" + (enemyPos.x - 0.5f);
            }
        }

        //ЛИПНЕМ В ЛЕВУЮ НИЖНЮЮ ЧАСТЬ
        else if (X > -0.7f * ballDiameter && X < 0 && Y <= 0.5

            && enemySide.BottomLeft == null)
        {
            this.transform.localPosition = new Vector3(enemyPos.x - 0.5f, enemyPos.y - 1, 0);
            this.name = (Math.Abs(enemyPos.y - 1)).ToString() + "x" + (enemyPos.x - 0.5f);

            //ДЛЯ КРАЙНИХ ШАРОВ
            if (enemyPos.x == 0)
            {
                this.transform.localPosition = new Vector3(enemyPos.x + 0.5f, enemyPos.y - 1, 0);
                this.name = (Math.Abs(enemyPos.y - 1)).ToString() + "x" + (enemyPos.x + 0.5f);
            }
        }
        //ШАР ЛИПНЕТ В ПРАВУЮ ЧАСТЬ
        else if (X > 0.7 * ballDiameter && X < 1.1f * ballDiameter && Y <= 0.5 && enemySide.Right == null)
        {
            this.transform.localPosition = new Vector3(enemyPos.x + 1f, enemyPos.y + 0, 0);
            this.name = (Math.Abs(enemyPos.y)).ToString() + "x" + (enemyPos.x + 1f);
        }
        //ШАР ЛИПНЕТ В ЛЕВУЮ ЧАСТЬ
        else if (X < -0.7 * ballDiameter && X > -1.1f * ballDiameter && Y <= 0.5 && enemySide.Left == null)
        {
            this.transform.localPosition = new Vector3(enemyPos.x - 1f, enemyPos.y + 0, 0);
            this.name = (Math.Abs(enemyPos.y)).ToString() + "x" + (enemyPos.x - 1f);
        }
        //ШАР ЛИПНЕТ В ПРАВУЮ ВЕРХНЮЮ ЧАСТЬ
        else if (X >= 0 && X < 1.1f * ballDiameter && Y > 0.5 && enemySide.TopRight == null)
        {
            this.transform.localPosition = new Vector3(enemyPos.x + 0.5f, enemyPos.y + 1, 0);
            this.name = (Math.Abs(enemyPos.y + 1)).ToString() + "x" + (enemyPos.x + 0.5f);
        }
        //ШАР ЛИПНЕТ В ЛЕВУЮ ВЕРХНЮЮ ЧАСТЬ
        else if (X > -1.1f * ballDiameter && X < 0 && Y > 0.5 && enemySide.TopLeft == null)
        {
            this.transform.localPosition = new Vector3(enemyPos.x - 0.5f, enemyPos.y + 1, 0);
            this.name = (Math.Abs(enemyPos.y + 1)).ToString() + "x" + (enemyPos.x - 0.5f);
        }


    }

    private float GetLastElemY()
    {
        Transform[] comp = GameObject.Find("Fields").GetComponentsInChildren<Transform>();
        return comp[comp.Length - 1].gameObject.GetComponent<Transform>().position.y;
    }

    private void Bounce(Collision col)
    {
        Vector3 velocity = this.GetComponent<Rigidbody>().velocity;

        //Отскок от стены шара (без использования Vector3.Reflect)
        foreach (ContactPoint contact in col.contacts)
        {
            //меняем вектор движения
            //например чтобы из (-2,0,0) сделать (2,0,0)
            //нужно (-2,0,0) - (-2,0,0)*2

            float dot = Vector3.Dot(velocity, Vector3.Normalize(contact.normal));
            Vector3 normal = Vector3.Normalize(contact.normal);//(1,0,0) X
            Vector3 v = 2 * dot * normal;
            velocity = velocity - v;

            this.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    private void FreezeBall()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void FindAndDeleteSingletons()
    {
        Transform[] comp = GameObject.Find("Fields").GetComponentsInChildren<Transform>();

        for (int i = 1; i < comp.Length; i++)
        {
            if (
                 comp[i].GetComponent<NeighboursAroundOfBall>().TopRight == null
                 &&
                 comp[i].GetComponent<NeighboursAroundOfBall>().TopLeft == null
                 &&
                 comp[i].GetComponent<NeighboursAroundOfBall>().Right == null
                 &&
                 comp[i].GetComponent<NeighboursAroundOfBall>().Left == null
                 &&
                 comp[i].GetComponent<NeighboursAroundOfBall>().BottomRight == null
                 &&
                 comp[i].GetComponent<NeighboursAroundOfBall>().BottomLeft == null
                 )
            {
                Destroy(comp[i].gameObject);
            }
        }

    }

    #endregion Methods
}

