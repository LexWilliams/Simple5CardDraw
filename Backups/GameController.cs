using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour{
	//declare public lists
	public List<GameObject> deck;
	public List<Vector3> cardPositions;
	public List<GameObject> playerHand = new List<GameObject>(5);
	public List<GameObject> dealerHand = new List<GameObject>(5);

	public List<Card> deckOfCards = new List<Card>();
	public List<Card> drawPileOfCards = new List<Card>();
	public List<Card> playerHandOfCards = new List<Card>();
	public List<Card> dealerHandOfCards = new List<Card>();

	//declare private lists
	private List<GameObject> drawPile = new List<GameObject>();
	private List<int> cardNumbers;

	//declare private variabes
	private GameObject currentCard;
	private int lastCardIndex;
	private int cardNumber;
	private int numberOfCardsToDraw;
	private int dealButtonPressCount;
	private int playerNumber;
	private Vector3 cardPosition;

	public void Start() {
		lastCardIndex = 12;
		dealButtonPressCount = 0;
		drawPile.AddRange (deck);
		for(int i = 0; i < 4; i++){
			for(int j = 0; j < 13; j++){
				Card card = new Card();
				card.Suite = i;
				card.Number = j;
				deckOfCards.Add (card);
			}
		}
		drawPileOfCards.AddRange (deckOfCards);
	}

	//picks number of random card from deck to deal
	private int CardNumber () {		
		lastCardIndex -= 1;
		return( Mathf.RoundToInt( Random.Range(0, drawPile.Count) ) );
	}

	private void DrawCard (Vector3 cardPosition, int playerNumber, int i) {
		switch (playerNumber){
		case 0:
			cardNumber = CardNumber ();
			dealerHand.Add (GameObject.Instantiate (drawPile [cardNumber], cardPosition, Quaternion.identity) as GameObject);
			dealerHandOfCards.Add (drawPileOfCards[cardNumber]);
			break;
		case 1:
			cardNumber = CardNumber ();
			playerHand.Add (GameObject.Instantiate (drawPile [cardNumber], cardPosition, Quaternion.identity) as GameObject);
			playerHandOfCards.Add (drawPileOfCards[cardNumber]);
			break;
		}
		drawPile.RemoveAt (cardNumber);	
		drawPileOfCards.RemoveAt (cardNumber);
	}

	public void DealButtonPress (){
		drawPile.Clear ();
		drawPile.AddRange (deck);
		drawPileOfCards.Clear ();
		drawPileOfCards.AddRange (deckOfCards);
		if (dealButtonPressCount > 0){
			lastCardIndex = 12;
			for (int i = 0; i < 5; i++){
				Destroy( playerHand[i] );
				Destroy( dealerHand[i] );
			}
			for (int i = 0; i < 5; i++){
				playerHand.RemoveAt (0);
				dealerHand.RemoveAt (0);
			}
		}
		for (int i = 0; i < 5; i++){
			cardNumber = CardNumber ();
			dealerHandOfCards.Add (new Card() );
			dealerHandOfCards [i].Suite = (cardNumber + 1) / 13;
			dealerHandOfCards [i].Number = (cardNumber + 1) / 4;

			cardNumber = CardNumber ();
			playerHandOfCards.Add (new Card() );
			playerHandOfCards [i].Suite = (cardNumber + 1) / 13;
			playerHandOfCards [i].Number = (cardNumber + 1) / 4;
		}
		for (int i = 0; i < 5; i++){
			cardPosition = cardPositions [5]; //sets position for dealer's card
			DrawCard(cardPosition, 0, i); //draws a card for the dealer
			dealerHand [i].transform.localScale -= new Vector3 (3.99999999f, 5.999999999f, 0.999999999999f); //makes dealer's card tiny
			cardPosition = cardPositions [i]; //sets position for player's card
			DrawCard(cardPosition, 1, i); //draws a card for player 1
		}
		dealButtonPressCount++;
	}

	public void ReplaceButtonPress(){
		
	}

	public void SelectCard1(){
		Debug.Log ("Button Pressed");
	}

	public void SelectCard2(){

	}

	public void SelectCard3(){

	}

	public void SelectCard4(){

	}

	public void SelectCard5(){

	}

	public void HandComparer(){

	}

	private void HandDeterminer(){

	}

	private bool RoyalFlush(){
		return true;
	}

	private bool StraightFlush(){
		return true;
	}

	private bool FourOfAKind(){
		return true;
	}

	private bool FullHouse(){
		return true;
	}

	private bool Flush(){
		return true;
	}

	private bool Straight(){
		return true;
	}

	private bool ThreeOfAKind(){
		return true;
	}

	private bool TwoPair(){
		return true;
	}

	private bool Pair(){
		return true;
	}

}

public class Card {
	private int suite = 0;
	private int number = 0;
	public int Suite {get {return suite;} set {suite = value;} }
	public int Number {get {return number;} set {number = value;} }
}
