using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	//declaring public variables
	public GameObject[] deckGraphics;
	public Vector3[] cardGraphicsPositions;
	public List<GameObject> playerHandGraphics = new List<GameObject> (5);

	//declaring private variables
	private List<Card> deck = new List<Card>();
	private List<Card> playerHand = new List<Card> ();
	private List<Card> dealerHand = new List<Card> ();
	private List<Card> drawPile = new List<Card> ();

	private int numberOfCardsToDraw;
	private int dealButtonPressCount;
	private int playerNumber;
	private int cardNumber;
	private Vector3 cardGraphicsPosition;


	private void Start(){
		dealButtonPressCount = 0;
		for(int i = 0; i < 52; i++){
			deck.Add (new Card());
			if (i < 13) {
				deck [i].Number = i;
			} else if (i >= 13 && i < 26) {
				deck [i].Number = i - 13;
			} else if (i >= 26 && i < 39){
				deck [i].Number = i - 26;
			} else{
				deck [i].Number = i - 39;
			}
			deck [i].Suite = i / 13;
			deck [i].CardGraphic = deckGraphics [i];
		}
		drawPile.AddRange (deck);
	}

	private int CardNumber () {		
		return( Mathf.RoundToInt( Random.Range(0, drawPile.Count) ) );
	}

	private void DrawCard(Vector3 cardPosition, int playerNumber, int i){
		cardNumber = CardNumber ();
		switch(playerNumber){
		case 0:
			dealerHand.Add (drawPile [cardNumber]);
			break;
		case 1:
			playerHand.Add (drawPile [cardNumber]);
			playerHandGraphics.Add (GameObject.Instantiate (playerHand[i].CardGraphic, cardPosition, Quaternion.identity) as GameObject);
			break;
		}
		drawPile.RemoveAt (cardNumber);
	}

	public void DealButtonPress(){
		drawPile.Clear ();
		drawPile.AddRange (deck);

		if(dealButtonPressCount > 0){
			
			for(int i = 0; i < 5; i++){
			Destroy (playerHandGraphics[i]);
			}
			dealerHand.RemoveRange (0, 5);
			playerHand.RemoveRange (0, 5);
		}

		for (int i = 0; i < 5; i++){
			cardGraphicsPosition = cardGraphicsPositions [i];
			DrawCard(cardGraphicsPosition, 0, i); //draws a card for the dealer
			//cardGraphicsPosition = cardGraphicsPositions [i]; //sets position for player's card
			DrawCard(cardGraphicsPosition, 1, i); //draws a card for player 1
		}

		dealButtonPressCount++;
	}



}

public class Card {
	private int suite = 0;
	private int number = 0;
	private GameObject cardGraphic = new GameObject();
	public int Suite {get {return suite;} set {suite = value;} }
	public int Number {get {return number;} set {number = value;} }
	public GameObject CardGraphic {get {return cardGraphic;} set {cardGraphic = value;} }
}