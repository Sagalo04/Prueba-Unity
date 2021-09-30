using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static float xin;
    public static float yin;

    // Start is called before the first frame update
    void Start()
    {
        xin = gameObject.transform.position.x;
        yin = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("left") && Time.timeScale == 1)
       {
            gameObject.GetComponent<Animator>().Play("Left");
            gameObject.transform.Translate(-10f * Time.deltaTime, 0, 0);
        }else if (Input.GetKeyUp("left"))
           
        {
            gameObject.GetComponent<Animator>().Play("ILeft");
        }

            
        if (Input.GetKey("right") && Time.timeScale == 1)
        {
            gameObject.GetComponent<Animator>().Play("Right");
            gameObject.transform.Translate(10f * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKeyUp("right"))
        {
            gameObject.GetComponent<Animator>().Play("IRight");
        }

        if (Input.GetKey("up") && Time.timeScale == 1)
        {
            gameObject.GetComponent<Animator>().Play("Back");
            gameObject.transform.Translate(0,10f * Time.deltaTime, 0); 
            
        }
        else if (Input.GetKeyUp("up"))
        {
            gameObject.GetComponent<Animator>().Play("IBack");
        }

        if (Input.GetKey("down") && Time.timeScale == 1)
            {
            gameObject.GetComponent<Animator>().Play("IFront");
            gameObject.transform.Translate(0,-10f * Time.deltaTime, 0);
            }
        else if (Input.GetKeyUp("down"))
        {
            gameObject.GetComponent<Animator>().Play("Front");
        }


    }


}
