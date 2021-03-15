using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D myBody;
    Animator myAnim;
    SpriteRenderer myRenderer;
    
    //BoxCollider2D myCollider;
    //public Sprite brokenPot;
    public Vector3 mousePos;
    private Vector3 dir;

    public float sightDist;
   [SerializeField] private bool hitNPC = false;
    public float distanceFromNPC;

    //private Vector2 mousePos;
    public LayerMask activatableLayer;
    public GameObject currentObj;
    public LayerMask NPCLayer;
    public GameObject currentNPC;
    GameObject hitnpc;
    GameObject hitobj;

    float moveDirX;
    float moveDirY;
    public float speed;
    public float speedLim = 0.7f;

    [SerializeField]int numOfBooks;
    public bool key = false;
    //public static bool faceRight = true;
    [SerializeField]int numOfQuests;
    
    string collectedBook = "You've picked up a book! Return it to the Librarian.";
    string cannotCollectBook = "Hm.. this doesn't look like any book you're looking for...";

    string maxedQuestsText = "You seem busy at the moment, come back later.";
    //NPC strings
    string lyingManText = "I left my book somewhere northwest,\nwhile I was admiring some flowers...\nI'm too tired. Could you return it for me?";
    bool lyingMan = false;

    string fancyManText = "Well golly, I must've dropped my book somewhere\nsoutheast, while I was admiring some flowers";
    bool fancyMan = false;

    string poshManText = "My book's in here but I'm locked out!\nA rabbit stole my keys while I was browsing the\nshelves. I saw it run north.";
    bool poshMan = false;

    string tallManText = "What? Where are you? Oh, oh down there.\nYou're looking for missing books huh? Well\nI dropped mine somewhere west of here.";
    bool tallMan = false;

    string shortManText = "Oh no I forgot to return the book to that\nLibrarian. I left it down the hall.";
    bool shortMan = false;

    string womanText = "Oh gosh I'm in a hurry! Books? Oh shoot, I left\nmine up northeast, when I was placing down flowers!";
    bool woman = false;

    string rabbitText = "Do you have the time? What? A key?\nThis key? Fine, you can have it.";
    bool rabbit = false;

    // Start is called before the first frame update
    
    
    void Start()
    {
        //myCollider = gameObject.GetComponent<BoxCollider2D>();
        mousePos = Vector3.zero;
        myBody = gameObject.GetComponent<Rigidbody2D>(); 
        myAnim = gameObject.GetComponent<Animator>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
    }

    void Update(){
        myRenderer.sortingOrder = -(int)Mathf.Floor(gameObject.transform.position.y);
        
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        //dir = (mousePos - transform.position).normalized;

        //Debug.DrawLine(transform.position, mousePos, Color.red, 1/60f);
        
        if(Input.GetMouseButtonDown(0)){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), mousePos);

            if(hit.collider != null){
                //Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.tag =="NPC"){
                    checkNPC(hit.collider.gameObject);
                }
                else if(hit.collider.gameObject.tag =="Activatable"){
                    checkActivatable(hit.collider.gameObject);
                }
            }
            
        }

        //if the player gets too far from the npc, close text
        if(hitNPC){
            if(Vector3.Distance(transform.position, currentNPC.transform.position)>= distanceFromNPC){
                gameManager.CloseTextBoxes();
                hitNPC = false;
            }
        }

        //open inventory
        if(Input.GetKey(KeyCode.I)){
            gameManager.ShowInventory("Inventory \n---------\nBooks: "+ numOfBooks);
            if(key){
                gameManager.ShowInventory(gameManager.inventoryText.text + "\n A key");
            }
        }
        // else{
        //     gameManager.CloseInfoBoxes();
        // }

        //open quest
        if(Input.GetKey(KeyCode.Q)){
            gameManager.ShowQuests("Quest Information\n---------\n");
            if(lyingMan){
            gameManager.ShowQuests(gameManager.questText.text + "Man on floor: " + lyingManText+"\n\n");
            }
            if(poshMan){
            gameManager.ShowQuests(gameManager.questText.text + "Posh man: " + poshManText+"\n\n");
            }
            if(fancyMan){
            gameManager.ShowQuests(gameManager.questText.text + "Fancy man: " + fancyManText+"\n\n");
            }
            if(tallMan){
            gameManager.ShowQuests(gameManager.questText.text + "Tall man: " + tallManText+"\n\n");
            }
            if(shortMan){
            gameManager.ShowQuests(gameManager.questText.text + "Short man: " + shortManText+"\n\n");
            }
            if(woman){
            gameManager.ShowQuests(gameManager.questText.text + "Woman: " + womanText+"\n\n");
            }

        }

        if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.Q)){
            gameManager.CloseInfoBoxes();
        }
        // else{
        //     gameManager.CloseInfoBoxes();
        // }

         

    }

    // void checkActivatable(){
    //     RaycastHit2D hitData = Physics2D.Raycast(transform.position, mousePos, sightDist, activatableLayer);
    //     if(hitData.collider != null){
    //         currentObj = hitData.collider.gameObject;
    //         displayActivatableText(currentObj.name);
    //     }
    // }

    void checkActivatable(GameObject hitobj){
        Debug.Log(hitobj.name);
        currentObj = hitobj;
        if(Vector3.Distance(transform.position, hitobj.transform.position)<= sightDist){
            displayActivatableText(hitobj.name);
        }
    }

    void displayActivatableText(string name){
        switch(name){
            case "FlowerPot":
            currentObj.GetComponent<flowerPot>().BreakPot();
            break;

            case "Book1":
                if(lyingMan){
                    gameManager.ShowNPCText(collectedBook);
                    Destroy(currentObj);
                    lyingMan = false;
                    numOfBooks++;
                    numOfQuests--;
                    Destroy(GameObject.Find("NPCs/LyingMan"));
                }
                else{
                    gameManager.ShowNPCText(cannotCollectBook);
                }
            break;
            case "Book2":
                if(poshMan){
                    gameManager.ShowNPCText(collectedBook);
                    Destroy(currentObj);
                    poshMan = false;
                    numOfBooks++;
                    numOfQuests--;
                    Destroy(GameObject.Find("NPCs/PoshMan"));
                }
                else{
                    gameManager.ShowNPCText(cannotCollectBook);
                }
            break;
            case "Book3":
                if(fancyMan){
                    gameManager.ShowNPCText(collectedBook);
                    Destroy(currentObj);
                    fancyMan = false;
                    numOfBooks++;
                    numOfQuests--;
                    Destroy(GameObject.Find("NPCs/FancyMan"));
                }
                else{
                    gameManager.ShowNPCText(cannotCollectBook);
                }
            break;
            case "Book4":
                if(tallMan){
                    gameManager.ShowNPCText(collectedBook);
                    Destroy(currentObj);
                    tallMan = false;
                    numOfBooks++;
                    numOfQuests--;
                    Destroy(GameObject.Find("NPCs/TallMan"));
                }
                else{
                    gameManager.ShowNPCText(cannotCollectBook);
                }
            break;
            case "Book5":
                if(shortMan){
                    gameManager.ShowNPCText(collectedBook);
                    Destroy(currentObj);
                    shortMan = false;
                    numOfBooks++;
                    numOfQuests--;
                    Destroy(GameObject.Find("NPCs/ShortMan"));
                }
                else{
                    gameManager.ShowNPCText(cannotCollectBook);
                }
            break;
            case "Book6":
                if(woman){
                    gameManager.ShowNPCText(collectedBook);
                    Destroy(currentObj);
                    woman = false;
                    numOfBooks++;
                    numOfQuests--;
                    Destroy(GameObject.Find("NPCs/Woman"));
                }
                else{
                    gameManager.ShowNPCText(cannotCollectBook);
                }
            break;
            case "Book7":
                gameManager.ShowNPCText(collectedBook);
                Destroy(currentObj);
                numOfBooks++;
            break;
            case "Lock":
                if(key){
                    Destroy(currentObj);
                }
            break;
        }
    }


    // void checkNPC(){

    //     RaycastHit2D hitData = Physics2D.Raycast(transform.position, mousePos, sightDist, NPCLayer);
    //     if(hitData.collider !=null){
    //         //if(Input.GetMouseDown(KeyCode.Space)){
    //         currentNPC = hitData.collider.gameObject;
    //         displayNPCText(currentNPC.name);
    //         Debug.Log(currentNPC.name);
    //         hitNPC = true;
    //         //}
    //     }
    // }
    void checkNPC(GameObject hitnpc){
        if(Vector3.Distance(transform.position, hitnpc.transform.position)<= sightDist){
            currentNPC = hitnpc;
            displayNPCText(currentNPC.name);
            Debug.Log(currentNPC.name);
            hitNPC = true;
        }
    }

    void displayNPCText(string npc){
        switch(npc){
            case "Librarian":
                //Debug.Log("Librarian hit");
                if(gameManager.booksLeft == gameManager.maxBooks && numOfBooks==0){
                    gameManager.ShowNPCText("Hey kid! Bring me "+ gameManager.booksLeft + " books, and I'll give you some snacks.");
                }
                else{
                    if(numOfBooks > 0){
                        gameManager.booksLeft = gameManager.booksLeft - numOfBooks;
                        if(gameManager.booksLeft == 0){
                            gameManager.ShowNPCText("Thanks kid!\nHere are some yummy snacks!");
                            Time.timeScale = 0;
                        }
                        else{
                            gameManager.ShowNPCText("ooo thanks for the " + numOfBooks + ((numOfBooks==1)? " book":" books") + "!\nBring me "+ gameManager.booksLeft + " more, and I'll give you some snacks. :)");
                        }
                        numOfBooks = 0;
                    }
                    else{
                        gameManager.ShowNPCText("Bring me "+ gameManager.booksLeft + " more books!\nI've got some delicious snacks waiting for you!");
                    }
                }
                
                break;

            case "LyingMan":
            if (!lyingMan){
                if(numOfQuests < gameManager.maxQuests){
                numOfQuests++;
                lyingMan = true;
                gameManager.ShowNPCText(lyingManText);
                }
                else{
                gameManager.ShowNPCText(maxedQuestsText);
                }

            }
                break;
            case "PoshMan":
            if (!poshMan){
                if(numOfQuests < gameManager.maxQuests){
                numOfQuests++;
                poshMan = true;
                gameManager.ShowNPCText(poshManText);
                }
                else{
                gameManager.ShowNPCText(maxedQuestsText);
                }

            }
                break;
            case "FancyMan":
                if (!fancyMan){
                if(numOfQuests < gameManager.maxQuests){
                numOfQuests++;
                fancyMan = true;
                gameManager.ShowNPCText(fancyManText);
                }
                else{
                gameManager.ShowNPCText(maxedQuestsText);
                }

            }
                break;
            case "TallMan":
                if (!tallMan){
                if(numOfQuests < gameManager.maxQuests){
                numOfQuests++;
                tallMan = true;
                gameManager.ShowNPCText(tallManText);
                }
                else{
                gameManager.ShowNPCText(maxedQuestsText);
                }

            }
                break;
            case "ShortMan":
                if (!shortMan){
                if(numOfQuests < gameManager.maxQuests){
                numOfQuests++;
                shortMan = true;
                gameManager.ShowNPCText(shortManText);
                }
                else{
                gameManager.ShowNPCText(maxedQuestsText);
                }

            }
                break;
            case "Woman":
                if (!woman){
                if(numOfQuests < gameManager.maxQuests){
                numOfQuests++;
                woman = true;
                gameManager.ShowNPCText(womanText);
                }
                else{
                gameManager.ShowNPCText(maxedQuestsText);
                }
            }
                break;
            case "Rabbit":
                if (!rabbit){
                if(poshMan){
                rabbit = true;
                gameManager.ShowNPCText(rabbitText);
                key = true;
                }
                else{
                gameManager.ShowNPCText("What's the time?");
                }
            }
                break;
        }
    }

    void move(){
        ResetAnim();
        moveDirX = Input.GetAxis("Horizontal");
        moveDirY = Input.GetAxis("Vertical");

        if(moveDirX > 0){
            myRenderer.flipX = false;
            myAnim.SetBool("walkingSide",true);
        }
        else if(moveDirX < 0){
            myRenderer.flipX = true;
            myAnim.SetBool("walkingSide",true);
        }
        // else{
        //     ResetAnim();
        // }

        //horizontal speed
        if (moveDirX !=0 && moveDirY !=0){
            moveDirX *= speedLim;
            moveDirY *= speedLim;
        }
        transform.Translate(Vector3.up*moveDirY*Time.deltaTime*speed);
        transform.Translate(Vector3.right*moveDirX*Time.deltaTime*speed);
        //myBody.velocity = new Vector2(moveDirX*speed, moveDirY*speed);
    }

    void ResetAnim(){
        myAnim.SetBool("walkingSide",false);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "book"){
            Renderer temp = (Renderer) other.gameObject.GetComponent<Renderer>();
            if(temp.enabled && other!=null){
                temp.enabled = false;
                Destroy(other.gameObject);
                numOfBooks++;
            }
        }
        // if(other.gameObject.tag == "pot"){
        //     pot potScript = (pot)other.gameObject.GetComponent(typeof(pot));
        //     SpriteRenderer potSpr = (SpriteRenderer)other.gameObject.GetComponent<SpriteRenderer>();

        //     potSpr.sprite = brokenPot;
        //     potScript.isBookVisible = true;
        // } 
    }

    //change "depth" of player
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if(other.gameObject.name !="World" && other.gameObject.tag !="NoDepth"){
    //         if(other.transform.position.y < transform.position.y){
    //             SpriteRenderer otherRenderer = (SpriteRenderer)other.gameObject.GetComponent<SpriteRenderer>();
    //             myRenderer.sortingLayerName = otherRenderer.sortingLayerName;
    //             myRenderer.sortingOrder = otherRenderer.sortingOrder -1;
    //         }
    //         else{
    //             myRenderer.sortingLayerName = "front";
    //             myRenderer.sortingOrder = 0;
    //         }
    //     }
        
    // }

    private void OnTriggerExit2D(Collider2D other)
    {
        myRenderer.sortingLayerName = "front";
        myRenderer.sortingOrder = 0;
    }

    //collision code
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "person_rabbit"){
            key = true;
        }

        if(other.gameObject.name =="gate" && key){
            Destroy(other.gameObject);
        }
    }
}
