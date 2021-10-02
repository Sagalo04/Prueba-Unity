using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField input;
    public Button btn;
    public static string user;
    public List<GameObject> BagAux = new List<GameObject>();


    public static List<Sprite> spritess = new List<Sprite>();
    public static List<string> types = new List<string>();
    public static List<string> atacks = new List<string>();
    public static List<string> defenses = new List<string>();
    public static List<string> luckys = new List<string>();
    public static List<bool> enables = new List<bool>();

    public static List<Sprite> Espritess = new List<Sprite>();
    public static List<string> Etypes = new List<string>();
    public static List<string> Eatacks = new List<string>();
    public static List<string> Edefenses = new List<string>();
    public static List<string> Eluckys = new List<string>();
    public static List<bool> Eenables = new List<bool>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(input.text != "")
        {
            btn.GetComponent<Button>().interactable = true;
        }
        else
        {
            btn.GetComponent<Button>().interactable = false;
        }
    }

    public void Onclick()
    {
        user = input.text;

        string url = $"http://127.0.0.1:5000/data/{user}";
        Debug.Log(url);
        RestClient.Get(url).Then(
        response =>
        {
            string S = response.Text;
        Debug.Log(S);
            var data = StringSerializationAPI.Deserialize(typeof(Atributos), response.Text) as Atributos;
            PlayerController.x = float.Parse(data.positionX);
            PlayerController.y = float.Parse(data.positionY);

            for (int i = 0; i < BagAux.Count; i++)
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/pixel_icons_by_oceansdream");
                var data2 = StringSerializationAPI.Deserialize(typeof(Item), data.items[i]) as Item;
                Sprite DaSprite = null;
                for (int j = 0; j < sprites.Length; j++)
                {

                    if (data2.Sprite == sprites[j].name)
                    {
                        DaSprite = sprites[j];
                        break;
                    }
                    else
                    {
                        DaSprite = null;
                    }
                }
                spritess.Add(DaSprite);
                atacks.Add(data2.Atack);
                defenses.Add(data2.Defense);
                luckys.Add(data2.Lucky);
                types.Add(data2.Type);

                if(DaSprite == null)
                {
                    enables.Add(false);
                }
                else{
                    enables.Add(true);
                }

            }

            for (int i = 0; i < 5; i++)
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/pixel_icons_by_oceansdream");
                var data2 = StringSerializationAPI.Deserialize(typeof(Item), data.Equipo[i]) as Item;
                Sprite DaSprite = null;
                for (int j = 0; j < sprites.Length; j++)
                {

                    if (data2.Sprite == sprites[j].name)
                    {
                        DaSprite = sprites[j];
                        break;
                    }
                    else
                    {
                        DaSprite = null;
                    }
                }
                Espritess.Add(DaSprite);
                Eatacks.Add(data2.Atack);
                Edefenses.Add(data2.Defense);
                Eluckys.Add(data2.Lucky);
                Etypes.Add(data2.Type);

                if (DaSprite == null)
                {
                    Eenables.Add(false);
                }
                else
                {
                    Eenables.Add(true);
                }

            }
            Debug.Log(S);
        }).Catch(Debug.Log);
        SceneManager.LoadScene(1);
    }
}
