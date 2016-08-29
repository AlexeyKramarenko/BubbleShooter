using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DynamicPartScript : MonoBehaviour
{
    #region Variables

    //фон текста проигрывания конца игры
    private GameObject image;

    //гейм-объект Fields
    private GameObject fieldsObj;

    private bool fallDown = false;
    private bool stopGame = false;
    private string createdSphere = "Sphere";

    #endregion Variables

    void Start()
    {
        image = GameObject.Find("Image");
        image.gameObject.SetActive(false);
        fieldsObj = GameObject.Find("Fields");
    }

    void Update()
    {
        if (fieldsObj.gameObject.GetComponent<Transform>().childCount == 0 && Shooting.shooting == false)
        {
            //победа           
            StartCoroutine(PlayVictory());
        }
        else if ((!LevelCheck() && !fallDown && !stopGame) || (LevelCheck() && stopGame))
        {
            //падение вниз плиты на один уровень
            StartCoroutine(FallDownByTime());
        }
        else if (GameObject.Find(createdSphere) != null && LevelCheck() && Shooting.shooting == false)
        {
            StartCoroutine(PlayGameOver());
        }
    }

    #region Methods

    //достиг ли один из верхних шаров уровня стреляющего шара
    private bool LevelCheck()
    {
        if (GameObject.Find(createdSphere))
            return Mathf.Abs(GameObject.Find(createdSphere).GetComponent<Transform>().position.y - FlyingBall.lastElY) < 1.0f;
        else
            return true;
    }

    private IEnumerator FallDownByTime()
    {
        fallDown = true;

        yield return new WaitForSeconds(5f);

        //опускаем потолок на уровень вниз
        this.GetComponent<Transform>().position += new Vector3(0, -1, 0);

        //опускаем висящие шарики на уровень вниз
        fieldsObj.GetComponent<Transform>().position += new Vector3(0, -1, 0);

        fallDown = false;
    }

    private IEnumerator PlayGameOver()
    {
        StopCoroutine(FallDownByTime());
        fallDown = true;
        stopGame = true;

        //запрет на стрельбу
        Shooting.shooting = true;

        yield return new WaitForSeconds(1f);

        image.GetComponent<Transform>().localScale = new Vector3(0.1f, 1, 1);
        image.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        image.GetComponent<Transform>().localScale = new Vector3(0.2f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.3f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.4f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.5f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.6f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.7f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.8f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.9f, 1, 1);
        yield return new WaitForSeconds(0.5f);
        image.GetComponent<Transform>().localScale = new Vector3(1f, 1, 1);
        image.GetComponentInChildren<Text>().resizeTextForBestFit = true;
        image.GetComponentInChildren<Text>().text = @"Game Over";
        yield return new WaitForSeconds(0.5f);
        image.GetComponent<Transform>().localScale = new Vector3(1f, 1, 1);
        image.GetComponentInChildren<Text>().text = @"Game Over                         You  ";
        yield return new WaitForSeconds(0.5f);
        image.GetComponent<Transform>().localScale = new Vector3(1f, 1, 1);
        image.GetComponentInChildren<Text>().text = @"Game Over                         You lose ";

    }

    private IEnumerator PlayVictory()
    {
        StopCoroutine(FallDownByTime());
        fallDown = true;
        stopGame = true;
        FlyingBall.startFindSingletons = false;

        //запрет на стрельбу
        Shooting.shooting = true;

        yield return new WaitForSeconds(1f);
        image.GetComponent<Image>().material.color = Color.yellow;
        image.GetComponent<Transform>().localScale = new Vector3(0.1f, 1, 1);
        image.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        image.GetComponent<Transform>().localScale = new Vector3(0.2f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.3f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.4f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.5f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.6f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.7f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.8f, 1, 1);
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Transform>().localScale = new Vector3(0.9f, 1, 1);
        yield return new WaitForSeconds(0.5f);
        image.GetComponent<Transform>().localScale = new Vector3(1f, 1, 1);
        image.GetComponentInChildren<Text>().resizeTextForBestFit = true;
        image.GetComponentInChildren<Text>().text = @"You win!";
    }

    #endregion Methods
}
