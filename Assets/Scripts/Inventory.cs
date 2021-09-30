using System.Collections;
using System.Collections.Generic;
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
                    Bag[i].GetComponent<Item>().Type = collision.GetComponent<Item>().Type;
                    Bag[i].GetComponent<Item>().Atack = collision.GetComponent<Item>().Atack;
                    Bag[i].GetComponent<Item>().Defense = collision.GetComponent<Item>().Defense;
                    Bag[i].GetComponent<Item>().Lucky = collision.GetComponent<Item>().Lucky;

                    break;
                }
            }
            Destroy(collision.gameObject);
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

                if (Input.GetKeyDown(KeyCode.Tab) && Activar_inv)
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

                if (Input.GetKeyDown(KeyCode.Tab) && Activar_inv)
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
                            /*if (Equipo[ID_equipo].GetComponent<Image>().enabled == false)
                            {
                                Equipo[ID_equipo].GetComponent<Image>().sprite = Bag[ID].GetComponent<Image>().sprite;
                                Equipo[ID_equipo].GetComponent<Image>().enabled = true;
                                Bag[ID].GetComponent<Image>().sprite = null;
                                Bag[ID].GetComponent<Image>().enabled = false;
                            }
                            else
                            {
                                Sprite obj = Bag[ID].GetComponent<Image>().sprite;
                                Debug.Log(Bag[ID].GetComponent<Image>().sprite.name);
                                Bag[ID].GetComponent<Image>().sprite = Equipo[ID_equipo].GetComponent<Image>().sprite;
                                Equipo[ID_equipo].GetComponent<Image>().sprite = obj;
                            }*/
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
            Equipo[id].GetComponent<Image>().enabled = true;
            Bag[ID].GetComponent<Image>().sprite = null;
            Bag[ID].GetComponent<Image>().enabled = false;
        }
        else
        {
            Sprite obj = Bag[ID].GetComponent<Image>().sprite;
            Debug.Log(Bag[ID].GetComponent<Image>().sprite.name);
            Bag[ID].GetComponent<Image>().sprite = Equipo[id].GetComponent<Image>().sprite;
            Equipo[id].GetComponent<Image>().sprite = obj;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
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
