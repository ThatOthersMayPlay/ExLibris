using UnityEngine;
using UnityEngine.SceneManagement;

//provided by Chatgpt
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance { get { return instance; } }

    private AudioSource audioSource;

    //scene to destroy this object:
    public static int destLv = -1;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {   //when the iven scene buildIndex is reached, the object and sound should be destroyed:
        if (destLv != -1 && SceneManager.GetActiveScene().buildIndex == destLv)
            StopMusic();
    }

    public void PlayMusic(AudioClip musicClip)
    {
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hier kannst du die Musik je nach Szene ändern, wenn gewünscht.
    }
}
