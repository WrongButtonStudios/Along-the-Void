using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class Timer : MonoBehaviour
{

    [SerializeField, Tooltip("max time the player has for the level in minutes")]
    private float _duration = 15;
    [SerializeField]
    private Slider _slider; //remove this when color drain effect is finnished 
    private float _durationInSeconds;
    private float _timeLeft; 
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2; 
        _durationInSeconds = _duration * 60f;
        _timeLeft = _durationInSeconds;
        _slider.maxValue = _durationInSeconds;
        _slider.minValue = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_timeLeft > 0)
        {
            _timeLeft -= Time.fixedDeltaTime;
            _slider.value = _timeLeft;
        }
        else
            SceneManager.LoadScene("LosingScreen"); 
    }
}
