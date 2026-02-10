using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenManEnem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public string targetTag = "Enemy"; // Define the tag to look for in the Inspector
    public string targetSceneName; // Define the scene name in the Inspector


     
     void LoadSceneByName(string Level1)
    {
        SceneManager.LoadScene(Level1);
    }
     
    

   

    void Start()
    {
        
    }

     // Update is called once per frame
    void Update()
    {
                int count = GameObject.FindGameObjectsWithTag(targetTag).Length;
        GameObject foundObject = GameObject.FindWithTag(targetTag);

        // Check if the found object is not null
        if (foundObject != null)
        {
            //There are still enemies on the scene.
            //Debug.LogWarning("Number of GameObjects with tag '" + targetTag + "': " + count);

            
        }
        else
        {
            LoadSceneByName(targetSceneName);
        }

    }
}
