using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> Bag = new List<GameObject>();
    public GameObject[] inv;
    public bool Activar_inv;

    public GameObject Selector;
    public int ID;

    public List<Image> Equipo = new List<Image>();
    public int ID_equipo;
    public int Fases_inv;

    public GameObject Opciones;
    public Image[] Seleccion;
    public Sprite[] Seleccion_Sprite;
    public int ID_select;

    bool cansave;
    public GameObject message;

    public TMP_Text Ataque;
    public TMP_Text Defensa;
    public TMP_Text Suerte;

    public TMP_Text name;

    public List<GameObject> objGame = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
      // if (collision.CompareTag("Item"))
       if (collision.gameObject.tag == "Item")
        {
            for (int i = 0; i < Bag.Count; i++)
            {
                if (Bag[i].GetComponent<Image>().enabled == false)
                {
                    Bag[i].GetComponent<Image>().enabled = true;
                    Bag[i].GetComponent<Image>().sprite = collision.GetComponent<SpriteRenderer>().sprite;
                    Bag[i].GetComponent<Item>().Sprite = collision.GetComponent<SpriteRenderer>().sprite.name;
                    Bag[i].GetComponent<Item>().Type = collision.GetComponent<Item>().Type;
                    Bag[i].GetComponent<Item>().Atack = collision.GetComponent<Item>().Atack;
                    Bag[i].GetComponent<Item>().Defense = collision.GetComponent<Item>().Defense;
                    Bag[i].GetComponent<Item>().Lucky = collision.GetComponent<Item>().Lucky;

                    break;
                }
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Save")
        {
            cansave = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Save")
        {
            cansave = false;
        }
    }

    private void Save()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var user = Login.user;
            var positionX = PlayerController.x;
            var positionY = PlayerController.y;

            List<string> items = new List<string>();
            List<string> itemsPlayer = new List<string>();

            for (int i = 0; i < Equipo.Count; i++)
            {
                itemsPlayer.Add($"{{\"Sprite\":\"{Equipo[i].GetComponent<Item>().Sprite}\",\"Type\":\"{Equipo[i].GetComponent<Item>().Type}\",\"Atack\":" +
                $"\"{Equipo[i].GetComponent<Item>().Atack}\",\"Defense\":\"{Equipo[i].GetComponent<Item>().Defense}\",\"Lucky\":\"{Equipo[i].GetComponent<Item>().Lucky}\"}}");

            }

            for (int i = 0; i < Bag.Count; i++)
            {
                items.Add($"{{\"Sprite\":\"{Bag[i].GetComponent<Item>().Sprite}\",\"Type\":\"{Bag[i].GetComponent<Item>().Type}\",\"Atack\":" +
                    $"\"{Bag[i].GetComponent<Item>().Atack}\",\"Defense\":\"{Bag[i].GetComponent<Item>().Defense}\",\"Lucky\":\"{Bag[i].GetComponent<Item>().Lucky}\"}}");

            }

            var itemsS = StringSerializationAPI.Serialize(typeof(List<string>), items);
            var itemsE = StringSerializationAPI.Serialize(typeof(List<string>), itemsPlayer);

            var payLoad = $"{{\"user\":\"{user}\",\"positionX\":\"{positionX}\",\"positionY\":" +
                    $"\"{positionY}\",\"items\":{itemsS},\"Equipo\":{itemsE}}}";

            RestClient.Post("http://127.0.0.1:5000/user", payLoad).Then(
                response =>
                {
                    string S = response.Text;
                    Debug.Log(S);
                }).Catch(Debug.Log);

        }
    }

    public void Navegar(){

        switch (Fases_inv)
        {
            case 0:
                Selector.SetActive(true);
                Opciones.SetActive(false);
                //inv[1].SetActive(false);
                if (Input.GetKeyDown(KeyCode.LeftArrow) && ID_equipo > 0) { ID_equipo--; }
                if (Input.GetKeyDown(KeyCode.RightArrow) && ID_equipo < Equipo.Count - 1) { ID_equipo++; }
                Selector.transform.position = Equipo[ID_equipo].transform.position;

                if (Input.GetKeyDown(KeyCode.UpArrow) && Activar_inv)
                {
                    Fases_inv = 1;
                }
                break;

            case 1:
                Selector.SetActive(true);
                Opciones.SetActive(false);

                if (Input.GetKeyDown(KeyCode.F) && Bag[ID].GetComponent<Image>().enabled == true)
                {
                    Fases_inv = 2;
                }

                //inv[1].SetActive(true);
                if (Input.GetKeyDown(KeyCode.RightArrow) && ID < 8) { ID++; }
                if (Input.GetKeyDown(KeyCode.LeftArrow) && ID > 0) { ID--; }
                if (Input.GetKeyDown(KeyCode.UpArrow) && ID > 2) { ID -= 3; }
                if (Input.GetKeyDown(KeyCode.DownArrow) && ID < 6) { ID += 3; }
                Selector.transform.position = Bag[ID].transform.position;

                if (ID >= 7 && ID <= 9 && Input.GetKeyDown(KeyCode.DownArrow) && Activar_inv)
                {
                    Fases_inv = 0;
                }
                break;

            case 2:
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Fases_inv = 1;
                }

                Opciones.SetActive(true);
                Opciones.transform.position = Bag[ID].transform.position;

                Selector.SetActive(false);

                if (Input.GetKeyDown(KeyCode.UpArrow) && ID_select > 0)
                {
                    ID_select--;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) && ID_select < Seleccion.Length - 1)
                {
                    ID_select++;
                }

                switch (ID_select)
                {
                    case 0:
                        Seleccion[0].sprite = Seleccion_Sprite[1];
                        Seleccion[1].sprite = Seleccion_Sprite[0];
                        Seleccion[2].sprite = Seleccion_Sprite[0];

                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            switch (Bag[ID].GetComponent<Item>().Type)
                            {
                                case "Extra":
                                    Almacenar(4);
                                    break;
                                case "Sword":
                                    Almacenar(0);
                                    break;
                                case "Armor":
                                    Almacenar(2);
                                    break;

                            }
                            Fases_inv = 0;
                        }
                        break;

                    case 1:
                        Seleccion[0].sprite = Seleccion_Sprite[0];
                        Seleccion[1].sprite = Seleccion_Sprite[1];
                        Seleccion[2].sprite = Seleccion_Sprite[0];

                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            Bag[ID].GetComponent<Image>().sprite = null;
                            Bag[ID].GetComponent<Image>().enabled = false;

                            Fases_inv = 1;
                        }
                        break;
                    case 2:
                        Seleccion[0].sprite = Seleccion_Sprite[0];
                        Seleccion[1].sprite = Seleccion_Sprite[0];
                        Seleccion[2].sprite = Seleccion_Sprite[1];

                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            Fases_inv = 1;
                        }
                        break;
                }

                break;
        }

    }

    public void Almacenar(int id)
    {
        if (Equipo[id].GetComponent<Image>().enabled == false)
        {
            Equipo[id].GetComponent<Image>().sprite = Bag[ID].GetComponent<Image>().sprite;
            Equipo[id].GetComponent<Item>().Sprite = Bag[ID].GetComponent<Image>().sprite.name;
            Equipo[id].GetComponent<Image>().enabled = true;
            Equipo[id].GetComponent<Item>().Type = Bag[ID].GetComponent<Item>().Type;
            Equipo[id].GetComponent<Item>().Atack = Bag[ID].GetComponent<Item>().Atack;
            Equipo[id].GetComponent<Item>().Defense = Bag[ID].GetComponent<Item>().Defense;
            Equipo[id].GetComponent<Item>().Lucky = Bag[ID].GetComponent<Item>().Lucky;

            Bag[ID].GetComponent<Image>().sprite = null;
            Bag[ID].GetComponent<Item>().Sprite = null;
            Bag[ID].GetComponent<Item>().Type = "";
            Bag[ID].GetComponent<Item>().Atack = "0";
            Bag[ID].GetComponent<Item>().Defense = "0";
            Bag[ID].GetComponent<Item>().Lucky = "0";
            Bag[ID].GetComponent<Image>().enabled = false;
        }
        else
        {
            Sprite obj = Bag[ID].GetComponent<Image>().sprite;
            string objType = Bag[ID].GetComponent<Item>().Type;
            string objsprite = Bag[ID].GetComponent<Item>().Sprite;
            string objAtack = Bag[ID].GetComponent<Item>().Atack;
            string objLucky = Bag[ID].GetComponent<Item>().Lucky;
            string objDefense = Bag[ID].GetComponent<Item>().Defense;

            Bag[ID].GetComponent<Image>().sprite = Equipo[id].GetComponent<Image>().sprite;
            Bag[ID].GetComponent<Item>().Type = Equipo[id].GetComponent<Item>().Type;
            Bag[ID].GetComponent<Item>().Atack = Equipo[id].GetComponent<Item>().Atack;
            Bag[ID].GetComponent<Item>().Defense = Equipo[id].GetComponent<Item>().Defense;
            Bag[ID].GetComponent<Item>().Lucky = Equipo[id].GetComponent<Item>().Lucky;

            Equipo[id].GetComponent<Image>().sprite = obj;
            Equipo[id].GetComponent<Item>().Sprite = objsprite;
            Equipo[id].GetComponent<Item>().Type = objType;
            Equipo[id].GetComponent<Item>().Atack = objAtack;
            Equipo[id].GetComponent<Item>().Defense = objDefense;
            Equipo[id].GetComponent<Item>().Lucky = objLucky;
        }

        switch (id)
        {
            case 0:
                Ataque.text = Equipo[id].GetComponent<Item>().Atack.ToString();
                break;
            case 2:
                Defensa.text = Equipo[id].GetComponent<Item>().Defense.ToString();
                break;
            case 4:
                Suerte.text = Equipo[id].GetComponent<Item>().Lucky.ToString();
                break;
            default:
                break;
        }
    }

    void Start()
    {
        //Debug.Log(Login.Bag.Count);
        for (int i = 0; i < Login.spritess.Count ; i++)
        {
            Bag[i].GetComponent<Image>().sprite = Login.spritess[i];
            if (Bag[i].GetComponent<Image>().sprite)
            {
                Bag[i].GetComponent<Item>().Sprite = Login.spritess[i].name;
            }
            else
            {
                Bag[i].GetComponent<Item>().Sprite = "";
            }
            Bag[i].GetComponent<Item>().Type = Login.types[i];
            Bag[i].GetComponent<Item>().Atack = Login.atacks[i];
            Bag[i].GetComponent<Item>().Defense = Login.defenses[i];
            Bag[i].GetComponent<Item>().Lucky = Login.luckys[i];
            Bag[i].GetComponent<Image>().enabled = Login.enables[i];
        }

        for (int i = 0; i < Login.Espritess.Count; i++)
        {
            Equipo[i].GetComponent<Image>().sprite = Login.Espritess[i];
            if (Equipo[i].GetComponent<Image>().sprite)
            {
                Equipo[i].GetComponent<Item>().Sprite = Login.Espritess[i].name;
            }
            else
            {
                Equipo[i].GetComponent<Item>().Sprite = "";
            }
            Equipo[i].GetComponent<Item>().Type = Login.Etypes[i];
            Equipo[i].GetComponent<Item>().Atack = Login.Eatacks[i];
            Equipo[i].GetComponent<Item>().Defense = Login.Edefenses[i];
            Equipo[i].GetComponent<Item>().Lucky = Login.Eluckys[i];
            Equipo[i].GetComponent<Image>().enabled = Login.Eenables[i];
        }

        for (int i = 0; i < objGame.Count; i++)
        {
            for (int j = 0; j < Bag.Count; j++)
            {
                if (objGame[i].GetComponent<SpriteRenderer>().sprite == Bag[j].GetComponent<Image>().sprite)
                {
                    Destroy(objGame[i]);
                }
            }
            for (int j = 0; j < Equipo.Count; j++)
            {
                if (objGame[i].GetComponent<SpriteRenderer>().sprite == Equipo[j].GetComponent<Image>().sprite)
                {
                    Destroy(objGame[i]);
                }
            }
        }
        name.text = Login.user;
        cansave = false;
    }

    void Update()
    {

        if (cansave)
        {
            message.SetActive(true);
            Save();
        }else
        {
            message.SetActive(false);
        }
        if (Activar_inv)
        {
        Navegar();
            Time.timeScale = 0;
            Selector.SetActive(true);
            inv[0].SetActive(true);
            inv[1].SetActive(true);
        }
        else
        {
            Fases_inv = 0;
            Time.timeScale = 1;
            Selector.SetActive(false);
            inv[0].SetActive(false);
            inv[1].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Activar_inv = !Activar_inv;
        }
    }
}
