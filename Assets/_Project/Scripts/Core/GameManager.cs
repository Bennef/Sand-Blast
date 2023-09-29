using Scripts.Audio;
using Scripts.Inputs;
using Scripts.UI;
using Scripts.Shooting;
using Scripts.Environment;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int _ballsLeft;
        [SerializeField] private int _blocksLeft;
        [SerializeField] private bool _gameOver, _levelClear, _gameComplete;
        private GameObject _castle;
        private Block[] _blocks;
        private Cannon _cannon;
        private float _delayTime = 0.2f;
        private float _fireTimer, _countDownTimer;
        private UIManager _uIManager;
        private SFXManager _sFXManager;
        private InputHandler _inputHandler;
        private Scene _scene;

        public bool GameOver { get => _gameOver; set => _gameOver = value; }
        
        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _ballsLeft = 10;
            _fireTimer = _delayTime;
            _countDownTimer = -1; // Set this to -1 because if it were 0, the GameOverCheck corountine would get called after countdown timer reaches 0.
            _levelClear = false;
            _cannon = GameObject.Find("Cannon").GetComponent<Cannon>();
            _castle = GameObject.Find("Castle");
            _blocks = _castle.GetComponentsInChildren<Block>();
            _scene = SceneManager.GetActiveScene();
            _uIManager = FindObjectOfType<UIManager>();
            _sFXManager = FindObjectOfType<SFXManager>();
            _inputHandler = FindObjectOfType<InputHandler>();
            UpdateBallCountUI();
            UpdateLevelText();
        }

        /// <summary>
        /// Set up references. Update the ball count and level text.
        /// </summary>
        /*void Start()
        {
            _ballsLeft = 10;
            _fireTimer = _delayTime;
            _countDownTimer = -1; // Set this to -1 because if it were 0, the GameOverCheck corountine would get called after countdown timer reaches 0.
            _cannon = GameObject.Find("Cannon").GetComponent<Cannon>();
            _castle = GameObject.Find("Castle");
            _blocks = _castle.GetComponentsInChildren<Block>();
            _scene = SceneManager.GetActiveScene();
            _uIManager = FindObjectOfType<UIManager>();
            _sFXManager = FindObjectOfType<SFXManager>();
            _inputHandler = FindObjectOfType<InputHandler>();
            UpdateBallCountUI();
            UpdateLevelText();
        }*/

        /// <summary>
        /// Update blocks, check for level clear, check for game over, handle input.
        /// </summary>
        void Update()
        {
            _fireTimer -= Time.deltaTime; // Use a delay to prevent spamming.
            CountBlocksLeft();
            UpdateBlockCountUI();

            if (_blocksLeft < 1 && !_levelClear && !_gameComplete)
            {
                _uIManager.HideText(_uIManager.CountdownTimerText);
                _uIManager.HidePanel(_uIManager.TimerPanel);

                if (_scene.name == "14")
                    GameComplete();
                else
                    LevelClear();
            }

            if (_ballsLeft < 1 && _blocksLeft > 0 && _gameOver == false)
            {
                if (_countDownTimer == -1)
                    StartCoroutine(GameOverCheck());
                else
                {
                    _countDownTimer -= Time.deltaTime;
                    _uIManager.SetText(_uIManager.CountdownTimerText, _countDownTimer.ToString().Substring(0, 3));
                }
            }
            HandleInput();            
        }

        /// <summary>
        /// Blocks are counted if they are above -3 on the y axis.
        /// </summary>
        private void CountBlocksLeft()
        {
            _blocksLeft = 0;
            
            foreach (Block block in _blocks)
            {
                if (!block.Cleared)
                    _blocksLeft += 1;
            }
        }

        /// <summary>
        /// Check if gameover, then if level clear, then if Game over, then if timer is ready allow firing.
        /// </summary>
        private void HandleInput()
        {
            if (_inputHandler.GetMousePress())
            {
                if (_gameComplete)
                    SceneManager.LoadScene("1");
                else if (_levelClear)
                    StartCoroutine(LoadNextScene());
                else if (_gameOver)
                    SceneManager.LoadScene(_scene.name);
                else if (_fireTimer < 0 && _ballsLeft > 0)
                {
                    _fireTimer = _delayTime;
                    _cannon.Fire();
                    UpdateBallCount();
                }
            }
        }

        /// <summary>
        /// Updates the number of balls the player has left. 
        /// </summary>
        private void UpdateBallCount()
        {
            _ballsLeft -= 1;
            UpdateBallCountUI();
        }

        /// <summary>
        /// Updates the number of active blocks left.
        /// </summary>
        private void UpdateBlockCountUI() => _uIManager.SetText(_uIManager.BlockCountText, _blocksLeft.ToString());//blockCountText.text = blocksLeft.ToString();

        /// <summary>
        /// Updates the UI label for number of balls left.
        /// </summary>
        private void UpdateBallCountUI() => _uIManager.SetText(_uIManager.BallCountText, _ballsLeft.ToString());

        /// <summary>
        /// Updates the UI for current level.
        /// </summary>
        private void UpdateLevelText() => _uIManager.SetText(_uIManager.LevelText, _scene.name);

        /// <summary>
        /// Check that blocks are still on the platfrom.
        /// </summary>
        private IEnumerator GameOverCheck()
        {
            _sFXManager.PlaySound(_sFXManager.TickTock);
            _countDownTimer = 2.7f;
            _uIManager.ShowText(_uIManager.CountdownTimerText);
            _uIManager.ShowPanel(_uIManager.TimerPanel);
            yield return new WaitForSeconds(2.7f); // Wait for 3 seconds to allow any blocks to fall off...
            _uIManager.HideText(_uIManager.CountdownTimerText);
            _uIManager.HidePanel(_uIManager.TimerPanel);

            if (_blocksLeft > 0 && !_levelClear)
            {
                _sFXManager.PlaySound(_sFXManager.Fail);
                _gameOver = true;
                _uIManager.ShowText(_uIManager.GameOverText);
                _uIManager.ShowPanel(_uIManager.GameOverPanel);
            }
        }

        /// <summary>
        /// Level is clear so hide the timer and show the level clear message.
        /// </summary>
        private void LevelClear()
        {
            if (_sFXManager.ASrc.isPlaying)
                _sFXManager.ASrc.Stop();
            _uIManager.HideText(_uIManager.CountdownTimerText);
            _uIManager.HidePanel(_uIManager.TimerPanel);
            _sFXManager.PlaySound(_sFXManager.Success);
            _uIManager.ShowText(_uIManager.LevelClearText);
            _uIManager.ShowPanel(_uIManager.LevelClearPanel);
            _levelClear = true;
        }

        /// <summary>
        /// Game is completed if we are in the final scene and all blocks are cleared.
        /// </summary>
        public void GameComplete()
        {
            _sFXManager.PlaySound(_sFXManager.GameCompleted);
            _uIManager.SetText(_uIManager.GameOverText , "Thanks for playing! Try again?");
            _uIManager.ShowText(_uIManager.GameOverText);
            _uIManager.ShowPanel(_uIManager.GameOverPanel);
            _gameComplete = true;
        }

        /// <summary>
        /// Asynchronously load next scene.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadNextScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
                yield return null;
        }
    }
}