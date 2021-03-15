using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerPot : MonoBehaviour
{
    public bool isBookVisible = false;
    public bool notDestroyed = true;
    BoxCollider2D myCollider;

    public Sprite brokenPot;

    public GameObject book;
    SpriteRenderer bookRenderer;

    SpriteRenderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = gameObject.GetComponent<BoxCollider2D>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();

        // bookRenderer = (SpriteRenderer)book.GetComponent<SpriteRenderer>();
        // bookRenderer.enabled = false;
        if(book !=null){
            book.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(book == null){
            notDestroyed = false;
        }

        if (isBookVisible && notDestroyed){
            // bookRenderer.enabled = true;
            book.SetActive(true);
        }
    }

    public void BreakPot(){
        myRenderer.sprite = brokenPot;
        myCollider.enabled = false;
        isBookVisible = true;

    }
}
