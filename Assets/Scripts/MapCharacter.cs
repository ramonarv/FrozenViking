using UnityEngine;
using UnityEngine.SceneManagement;

public class MapCharacter : MonoBehaviour
{

    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // When the map is loaded, we ask game manager if it has some info about currentLevel
        // if it has, then we know we are coming back from some level
        // so we can fetch the object and the spawnpoint of it
        // we move MapCharacter to that location
        if(GameManager.manager.currentLevel != "")
        {
            // We move the player to spawnpoint location
            transform.position = GameObject.Find(GameManager.manager.currentLevel).
                transform.GetChild(1).transform.position;
            GameObject.Find(GameManager.manager.currentLevel).GetComponent<LoadLevel>().Cleared(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float verticalMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(horizontalMove, verticalMove, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelTrigger"))
        {
            // We tell game manager that we are entering a level. GM will keep the information
            // the information is the name of the object we hit
            GameManager.manager.currentLevel = collision.gameObject.name;

            // We hit leveltrigger. we fetch the level name from the trigger's component
            SceneManager.LoadScene(collision.GetComponent<LoadLevel>().levelToLoad);

        }
    }
}
