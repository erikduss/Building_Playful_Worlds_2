using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestController : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject closedChest;
    public GameObject openChestBottom;
    public GameObject openChestTop;

    public bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openThisChest()
    {
        closedChest.SetActive(false);
        openChestTop.SetActive(true);
        openChestBottom.SetActive(true);
        opened = true;
        gameManager.showChestOpenText(false, null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !opened)
        {
            gameManager.showChestOpenText(true, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !opened)
        {
            gameManager.showChestOpenText(false, null);
        }
    }
}
