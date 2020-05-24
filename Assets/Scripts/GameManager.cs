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
        private int blocksLeft = 0;
        [SerializeField]
        private int cannonBallsLeft;

        private GameObject castle;
        private Cannon cannon;

        private Text blockCountText, ballCountText, gameOverText, levelClearText;
 
        private float time = 0.2f;
        private float timer;

        private bool gameOver, levelClear = false;

        // Use this for initialization
        void Start()
        {
            timer = time;

            cannon = GameObject.Find("Cannon").GetComponent<Cannon>();
            blockCountText = GameObject.Find("Blocks Left").GetComponent<Text>();
            gameOverText = GameObject.Find("Game Over Label").GetComponent<Text>();
            ballCountText = GameObject.Find("Balls Left").GetComponent<Text>();
            levelClearText = GameObject.Find("Level Clear Label").GetComponent<Text>();

            castle = GameObject.Find("Castle");

            foreach (Transform child in castle.transform)
            {
                blocksLeft += 1;
            }
            UpdateBlockCount();
            UpdateBlockCountUI();

            UpdateBallCountUI();
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime; // Use a delay to prevent spamming.

            UpdateBlockCount();
            UpdateBlockCountUI();

            if (blocksLeft < 1)
            {
                LevelClear();
            }

            if (levelClear && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(LoadNextScene());
            }

            // If there are no balls left and any blocks...
            if (cannonBallsLeft == 0 && blocksLeft > 0)
            {
                GameOver();
            }

             HandleInput();
        }

        /// <summary>
        /// Blocks are counted if they are above -3 on the y axis.
        /// </summary>
        private void UpdateBlockCount()
        {
            blocksLeft = 0;
            var blocks = castle.GetComponentsInChildren<Block>();

            foreach (Block block in blocks)
            {
                if (block.isActive)
                {
                    blocksLeft += 1;
                }
            }
        }


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
                    StartCoroutine(ReloadScene());
                }
                else if(timer < 0)
                {
                    timer = time;
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


        private void GameOver()
        {
            gameOver = true;
            gameOverText.enabled = true;
        }


        private void LevelClear()
        {
            levelClear = true;
            levelClearText.enabled = true;
        }


        private IEnumerator ReloadScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().ToString());

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
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