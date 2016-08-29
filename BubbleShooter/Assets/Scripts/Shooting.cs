using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

//скрипт висит на объекте Shooter
public class Shooting : MonoBehaviour
{
    #region Variables
    //флаг, запрещающий стрелять по шарикам пока ранее выпущенный находится в полете
    [HideInInspector]
    public static bool shooting = false;
    public GameObject _object = null;
    
    private Color fireBallcolor;    

    private Color[] colors = new Color[]{
     Color.blue,
     Color.grey,
     Color.green,
     Color.red,
     Color.yellow,
     Color.white
   };

    #endregion Variables


    void Start()
    {
        fireBallcolor = colors[Random.Range(0, 6)];//раскрашивание шаров
        Appear();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && shooting == false)
        {
            Shoot();
            fireBallcolor = colors[Random.Range(0, 6)];
        }
    }

    void Update()
    {
        if (GameObject.Find("Sphere") == null)
        {
            Appear();
        }
    }


    #region Methods

    //появление шарика
    private void Appear()
    {
        GameObject projectile = (GameObject)Instantiate(_object, this.GetComponent<Transform>().position, Quaternion.identity);      
        projectile.GetComponent<Transform>().SetParent(GameObject.Find("MainCamera").GetComponent<Transform>());
        projectile.GetComponent<Renderer>().material.color = fireBallcolor;
        projectile.name = "Sphere";
    }

    private void Shoot()
    {
        shooting = true;//после коллизии будет опять false 
        GameObject projectile = GameObject.Find("Sphere");
        projectile.GetComponent<Transform>().SetParent(GameObject.Find("Fields").GetComponent<Transform>());
        projectile.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 14, 0));

    }

    #endregion Methods

}
