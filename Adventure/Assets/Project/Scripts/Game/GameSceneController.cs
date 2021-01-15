using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
	[Header("Game")]
	public Player player;
	public GameCamera gameCamera;

	[Header("UI")]
	public GameObject[] hearts;
	public Text bombText;
	public Text arrowText;
	public Text orbText;
	public GameObject gameOverLabel;
	public GameObject dungeonPanel;
	public Text dungeonInfoText;

	private float resetTimer = 3f;
	
    // Start is called before the first frame update
    void Start()
    {
		Startup();
    }

    // Update is called once per frame
    void Update()
    {
		if (player != null)
		{
			// Check for Player Information
			for( int i = 0; i < hearts.Length; i++)
			{
				hearts[i].SetActive( i < player.health );
			}

			bombText.text = "Bombs: " + player.bombAmount;
			arrowText.text = "Arrows: " + player.arrowAmount;
			orbText.text = "Orbs: " + player.orbsAmount;

			// Check for Dungeon Information
			Dungeon currentDungeon = player.CurrentDungeon;
			dungeonPanel.SetActive(currentDungeon != null);
			if ( currentDungeon != null)
			{
				float enemiesPercentage = (float)(currentDungeon.EnemyCount - currentDungeon.CurrentEnemyCount) / currentDungeon.EnemyCount;
				dungeonInfoText.text = "Progress: " + Mathf.FloorToInt(enemiesPercentage*100) + "%";

				if( currentDungeon.JustCleared )
				{
					//gameCamera.FocusOn(currentDungeon.DungeonTreasure.gameObject);
				}
			}
		}
		else {
			for (int i = 0; i < hearts.Length; i++)
			{
				hearts[i].SetActive(false);
			}

			resetTimer -= Time.deltaTime;
			if( resetTimer <= 0.0f)
			{
				SceneManager.LoadScene("Menu");
			}
		}

		if (player.health <= 0)
		{
			gameOverLabel.SetActive(true);
		}
    }

	private void Startup()
	{
		if( player.health > 0)
		{
			gameOverLabel.SetActive(false);
			//gameOverLabel.transform.localScale = gameOverLabel.transform.localScale + new Vector3(5, 5, 5);
		}
	}
}
