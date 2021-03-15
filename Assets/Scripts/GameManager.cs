using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // public GameObject player;
    // player playerScript; //script player is using
    public int maxBooks = 7;
    public int maxQuests = 2;
    public int booksLeft;

    public GameObject questTextBox;
    //public GameObject questTextObj;
    public Text questText;

    public GameObject inventoryTextBox;
    //public GameObject inventoryTextObj;
    public Text inventoryText;


    public GameObject NPCTextBox;
    //public GameObject NPCTextObj;
    public Text NPCText;
    bool openText = false;

    void Start(){
        ShowStartText();
        booksLeft = maxBooks;
        // playerScript = (player)player.gameObject.GetComponent(typeof(player));
        GameObject [] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        GameObject[] activatables = GameObject.FindGameObjectsWithTag("Activatable");
        // if(npcs[0] !=null){
        //     Debug.Log(npcs.Length);
        // }
        // else Debug.Log("no items");
        
        foreach (GameObject o in npcs){
            Debug.Log(o.gameObject.name);
            o.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)Mathf.Floor(o.transform.position.y);
            // SpriteRenderer oR = (SpriteRenderer)o.gameObject.GetComponent<SpriteRenderer>();
            // oR.sortingOrder = oR.sortingOrder -1;
        }
        foreach (GameObject o in activatables){
            o.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)Mathf.Floor(o.transform.position.y);
        }
        foreach (GameObject o in walls){
            o.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)Mathf.Floor(o.transform.position.y);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(openText){
            //Debug.Log("Open Text");
            if(Input.GetKeyDown(KeyCode.Escape)){
                CloseTextBoxes();
            }
        }

        // if(playerScript.key){
        //     inventoryText.text = "BOOKS: " + playerScript.numOfBooks + "\n KEY";
        // }
        // else{
        //     inventoryText.text = "BOOKS: " + playerScript.numOfBooks;
        // }
    }

    public void CloseTextBoxes(){
                //Debug.Log("Hit ESC");
                // questTextBox.SetActive(false);
                // inventoryTextBox.SetActive(false);
                NPCTextBox.SetActive(false);
                openText = false;
    }
    public void CloseInfoBoxes(){
        questTextBox.SetActive(false);
        inventoryTextBox.SetActive(false);
        //openText = false;
    }

    private void ShowStartText(){
        NPCTextBox.SetActive(true);
        NPCText.text = "Welcome!\n\n< Left Click > an NPC/object when you're near them to interact.\n\nPress < ESC > to leave dialogue.\n\nHold < I > for inventory & < Q > for quest information.";
        openText = true;
    }

    public void ShowInventory(string text){
        inventoryTextBox.SetActive(true);
        inventoryText.text = text;
        openText = true;
    }
    
    public void ShowQuests(string text){
        questTextBox.SetActive(true);
        questText.text = text;
        openText = true;

    }

    public void ShowNPCText(string text){
        NPCTextBox.SetActive(true);
        NPCText.text = text;
        openText = true;
    }
}
