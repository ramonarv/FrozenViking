using UnityEngine;

public class LoadLevel : MonoBehaviour
{

    public string levelToLoad; // name of the scene we want to open

    public bool cleared;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // When we open Map scene, we check has Game manager marked this level as passed
        // If it is passed, then we run Cleared function with parameter true
        // That will display Level cleared image and remove collider


        if (GameManager.manager.GetType().GetField(levelToLoad).GetValue(GameManager.manager).ToString() == "True")
        {
            Cleared(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cleared(bool IsClear)
    {
        if(IsClear == true)
        {
            cleared = true;
            // We set correct boolean variable true in Game Manager
            GameManager.manager.GetType().GetField(levelToLoad).SetValue(GameManager.manager, true);

            // Display Level Clear sign
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // Because the level is passed, we want to disable collider, so we don't end up back to
            // already passed level
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
