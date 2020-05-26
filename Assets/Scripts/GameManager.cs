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

        private Text blockCountText, ballCountText, gameOverText, levelClearText, countdownTimerText;
 
        private float delayTime = 0.2f;
        private float fireTimer, countDownTimer;

        private bool gameOver, levelClear = false;

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

            blockCountText = GameObject.Find("Blocks Left").GetComponent<Text>();
            gameOverText = GameObject.Find("Game Over Label").GetComponent<Text>();
            ballCountText = GameObject.Find("Balls Left").GetComponent<Text>();
            levelClearText = GameObject.Find("Level Clear Label").GetComponent<Text>();
            countdownTimerText = GameObject.Find("Count Down Timer").GetComponent<Text>();

            UpdateBallCountUI();
        }

        /// <summary>
        /// Update blocks, check for level clear, check for game over, handle input.
        /// </summary>
        void Update()
        {
            fireTimer -= Time.deltaTime; // Use a delay to prevent spamming.

            CountBlocksLeft();
            UpdateBlockCountUI();

            if (blocksLeft < 1)
            {
                LevelClear();
            }

            // If there are no balls left and any blocks...
            if (cannonBallsLeft < 1 && blocksLeft > 0)
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
                if (block.isActive)
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
                if (levelClear)
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


        private void UpdateBallCount()
        {
            cannonBallsLeft -= 1;
            UpdateBallCountUI();
        }


        private void UpdateBlockCountUI()
        {
            blockCountText.text = blocksLeft.ToString();
        }


        private void UpdateBallCountUI()
        {
            ballCountText.text = cannonBallsLeft.ToString();
        }

        /// <summary>
        /// Check that blocks are still on the platfrom.
        /// </summary>
        private IEnumerator GameOverCheck()
        {
            countDownTimer = 3;
            
            countdownTimerText.enabled = true;
            // Wait for 3 seconds to allow any blocks to fall off...
            yield return new WaitForSeconds(3);
            countdownTimerText.enabled = false;
            if (blocksLeft > 0 && !levelClear)
            {
                gameOver = true;
                gameOverText.enabled = true;
            }
        }


        private void LevelClear()
        {
            levelClear = true;
            countdownTimerText.enabled = false;
            levelClearText.enabled = true;
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        private void ReloadScene()
        {
            Scene scene = SceneManager.GetActiveScene();
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
    }
}