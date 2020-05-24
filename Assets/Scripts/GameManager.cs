using UnityEngine;
using UnityEngine.UI;

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

        private Text blockCountText;
        private Text ballCountText;
        private Text gameOverText;

        private float time = 0.2f;
        private float timer;

        private bool gameOver = false;

        // Use this for initialization
        void Start()
        {
            timer = time;

            cannon = GameObject.Find("Cannon").GetComponent<Cannon>();
            blockCountText = GameObject.Find("Blocks Left").GetComponent<Text>();
            gameOverText = GameObject.Find("Game Over Label").GetComponent<Text>();
            ballCountText = GameObject.Find("Balls Left").GetComponent<Text>();

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

            // If there are no balls left and any blocks...
            if (cannonBallsLeft == 0 && blocksLeft > 0)
            {
                GameOver();
            }

            if (Input.GetMouseButtonDown(0) && timer < 0 && !gameOver)
            {
                timer = time;
                cannon.Fire();
                UpdateBallCount();
            }
        }

        /// <summary>
        /// Blocks are counted if they are above -3 on the y axis.
        /// </summary>
        public void UpdateBlockCount()
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


        public void UpdateBallCount()
        {
            cannonBallsLeft -= 1;
            UpdateBallCountUI();
        }


        public void UpdateBlockCountUI()
        {
            blockCountText.text = blocksLeft.ToString();
        }


        public void UpdateBallCountUI()
        {
            ballCountText.text = cannonBallsLeft.ToString();
        }


        public void GameOver()
        {
            gameOver = true;
            gameOverText.enabled = true;
        }
    }
}