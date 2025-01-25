using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class Timer : MonoBehaviour
{

    public static float TimeInPercent; 

    [SerializeField, Tooltip("max time the player has for the level in minutes")]
    private float _duration = 15;


    [Header("Debugging")]
    [SerializeField]
    private float _timeScale = 1; 
    private float _durationInSeconds;
    private float _timeLeft; 
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = _timeScale; 
        _durationInSeconds = _duration * 60f;
        _timeLeft = _durationInSeconds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_timeLeft > 0)
        {
            _timeLeft -= Time.fixedDeltaTime;
            TimeInPercent = _timeLeft / _durationInSeconds; 
        }
        else
            SceneManager.LoadScene("LosingScreen"); 
    }
}
