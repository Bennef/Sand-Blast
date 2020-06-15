using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SandBlast
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int currentLevel;
        private int blocksLeft;
        [SerializeField]
        private int cannonBallsLeft;

        private GameObject castle;
        private Cannon cannon;

        Block[] blocks;

        private Text levelText, blockCountText, ballCountText, gameOverText, levelClearText, countdownTimerText;
        private Image gameOverPanel, levelClearPanel, timerPanel;

        private float delayTime = 0.2f;
        private float fireTimer, countDownTimer;

        private bool gameOver, levelClear, gameComplete = false;

        private AudioSource aSrc;
        public AudioClip tickTock, fail, success, gameCompleted;

        Scene scene;


        /// <summary>
        /// Set up references.
        /// </summary>
        void Start()
        {
            fireTimer = delayTime;
            countDownTimer = -1; // Set this to -1 because if it were 0, the GameOverCheck corountine would get called after countdown timer reaches 0.

            cannon = GameObject.Find("Cannon").GetComponent<Cannon>();
            castle = GameObject.Find("Castle");
            blocks = castle.GetComponentsInChildren<Block>();
            
            levelText = GameObject.Find("Level Text").GetComponent<Text>();
            gameOverText = GameObject.Find("Game Over Label").GetComponent<Text>();
            blockCountText = GameObject.Find("Blocks Left").GetComponent<Text>();
            gameOverText = GameObject.Find("Game Over Label").GetComponent<Text>();
            ballCountText = GameObject.Find("Balls Left").GetComponent<Text>();
            levelClearText = GameObject.Find("Level Clear Label").GetComponent<Text>();
            countdownTimerText = GameObject.Find("Count Down Timer").GetComponent<Text>();

            gameOverPanel = GameObject.Find("Game Over Panel").GetComponent<Image>();
            levelClearPanel = GameObject.Find("Level Clear Panel").GetComponent<Image>();
            timerPanel = GameObject.Find("Timer Panel").GetComponent<Image>();

            aSrc = gameObject.GetComponent<AudioSource>();

            scene = SceneManager.GetActiveScene();

            UpdateBallCountUI();
            UpdateLevelText();
        }

        /// <summary>
        /// Update blocks, check for level clear, check for game over, handle input.
        /// </summary>
        void Update()
        {
            fireTimer -= Time.deltaTime; // Use a delay to prevent spamming.

            CountBlocksLeft();
            UpdateBlockCountUI();

            if (blocksLeft < 1 && !levelClear && !gameComplete)
            {
                countdownTimerText.enabled = false;
                timerPanel.enabled = false;

                if (scene.name == "14")
                {
                    GameComplete();
                }
                else
                {
                    LevelClear();
                }
            }

            // If there are no balls left and any blocks...
            if (cannonBallsLeft < 1 && blocksLeft > 0 && gameOver == false)
            {
                if (countDownTimer == -1)
                {
                    StartCoroutine(GameOverCheck());
                }
                else
                {
                    countDownTimer -= Time.deltaTime;
                    countdownTimerText.text = countDownTimer.ToString().Substring(0, 3);
                }
            }
            HandleInput();            
        }

        /// <summary>
        /// Blocks are counted if they are above -3 on the y axis.
        /// </summary>
        private void CountBlocksLeft()
        {
            blocksLeft = 0;
            
            foreach (Block block in blocks)
            {
                if (!block.cleared)
                {
                    blocksLeft += 1;
                }
            }
        }

        /// <summary>
        /// Check iff level clear, then if Game over, then if timer is ready allow firing.
        /// </summary>
        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gameComplete)
                {
                    RestartGame();
                }
                else if (levelClear)
                {
                    StartCoroutine(LoadNextScene());
                }
                else if (gameOver)
                {
                    ReloadScene();
                }
                else if (fireTimer < 0 && cannonBallsLeft > 0)
                {
                    fireTimer = delayTime;
                    cannon.Fire();
                    UpdateBallCount();
                }
            }
        }

        /// <summary>
        /// Updates the number of balls the player has left. 
        /// </summary>
        private void UpdateBallCount()
        {
            cannonBallsLeft -= 1;
            UpdateBallCountUI();
        }

        /// <summary>
        /// Updates the number of active blocks left.
        /// </summary>
        private void UpdateBlockCountUI()
        {
            blockCountText.text = blocksLeft.ToString();
        }

        /// <summary>
        /// Updates the UI label for number of balls left.
        /// </summary>
        private void UpdateBallCountUI()
        {
            ballCountText.text = cannonBallsLeft.ToString();
        }

        /// <summary>
        /// Updates the UI for current level.
        /// </summary>
        private void UpdateLevelText()
        {
            levelText.text = scene.name;
        }

        /// <summary>
        /// Check that blocks are still on the platfrom.
        /// </summary>
        private IEnumerator GameOverCheck()
        {
            countDownTimer = 2.7f;
            
            countdownTimerText.enabled = true;
            timerPanel.enabled = true;
            PlaySound(tickTock);
            // Wait for 3 seconds to allow any blocks to fall off...
            yield return new WaitForSeconds(2.7f);
            countdownTimerText.enabled = false;
            timerPanel.enabled = false;
            if (blocksLeft > 0 && !levelClear)
            {
                PlaySound(fail);

                gameOver = true;
                gameOverText.enabled = true;
                gameOverPanel.enabled = true;
            }
        }

        /// <summary>
        /// Level is clear so hide the timer and show the level clear message.
        /// </summary>
        private void LevelClear()
        {
            PlaySound(success);
            
            levelClear = true;
            levelClearText.enabled = true;
            levelClearPanel.enabled = true;
        }

        /// <summary>
        /// Game is completed if we are in the final scene and all blocks are cleared.
        /// </summary>
        public void GameComplete()
        {
            PlaySound(gameCompleted);

            gameOverText.text = "Thanks for playing! Try again?";
            gameOverText.enabled = true;
            gameOverPanel.enabled = true;
            gameComplete = true;
        }

        /// <summary>
        /// Restart the gane from scene 1.
        /// </summary>
        private void RestartGame()
        {
            SceneManager.LoadScene("1");
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        private void ReloadScene()
        {
            SceneManager.LoadScene(scene.name);
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
            {
                yield return null;
            }
        }

        /// <summary>
        /// Play the passed AudioClip.
        /// </summary>
        public void PlaySound(AudioClip clip)
        {
            aSrc.clip = clip;
            aSrc.Play();
        }
    }
}