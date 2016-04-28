using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	//declaring public variables
	public GameObject[] deckGraphics;
	public Vector3[] cardGraphicsPositions;
	public List<GameObject> playerOneHandGraphics = new List<GameObject> (5);

	//declaring private variables
	private List<Card> deck = new List<Card>();
	private List<Card> playerOneHand = new List<Card> ();
	private List<Card> dealerHand = new List<Card> ();
	private List<Card> drawPile = new List<Card> ();
	private List<int> handRanking = new List<int>();
	private List<int> dealerHandRank = new List<int>();
	private List<int> playerOneHandRank = new List<int>();
	private int numberOfCardsToDraw;
	private int dealButtonPressCount;
	private int playerNumber;
	private int cardNumber;
	private int winningPlayerNumber;

	private Vector3 cardGraphicsPosition;

	private void Start(){
		dealButtonPressCount = 0;
		for(int i = 0; i < 52; i++){
			if (i < 13) {
				deck.Add (new Card(i / 13, i, deckGraphics[i]));
			} else if (i >= 13 && i < 26) {
				deck.Add (new Card(i / 13, i - 13, deckGraphics[i]));
			} else if (i >= 26 && i < 39){
				deck.Add (new Card(i / 13, i - 26, deckGraphics[i]));
			} else{
				deck.Add (new Card(i / 13, i - 39, deckGraphics[i]));
			}
		}
		drawPile.AddRange (deck);
	}

	//picks number of random card from deck to deal
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
			playerOneHand.Add (drawPile [cardNumber]);
			playerOneHandGraphics.Add (GameObject.Instantiate (playerOneHand[i].CardGraphic, cardPosition, Quaternion.identity) as GameObject);
			break;
		}
		drawPile.RemoveAt (cardNumber);
	}

	public void DealButtonPress(){
		drawPile.Clear ();
		drawPile.AddRange (deck);
		if(dealButtonPressCount > 0){
			for(int i = 0; i < 5; i++){
			Destroy (playerOneHandGraphics[i]);
			}
			dealerHand.RemoveRange (0, 5);
			playerOneHand.RemoveRange (0, 5);
			playerOneHandGraphics.RemoveRange (0, 5);
		}
		for (int i = 0; i < 5; i++){
			cardGraphicsPosition = cardGraphicsPositions [i]; //sets position for player's card
			DrawCard(cardGraphicsPosition, 1, i); //draws a card for player 1
			DrawCard(cardGraphicsPosition, 0, i); //draws a card for the dealer
		}
		dealButtonPressCount++;
	}

	public void ReplaceButtonPress(){
		int winningPlayerNumber = 0;
		winningPlayerNumber = HandComparer ();
		if (winningPlayerNumber == 0){
			Debug.Log ("Dealer wins with a hand ranking of " + dealerHandRank + "!");
			dealerHandRank.Clear ();
			playerOneHandRank.Clear ();
		}
		else{
			Debug.Log ("Player One wins with a hand ranking of " + playerOneHandRank + "!");
			dealerHandRank.Clear ();
			playerOneHandRank.Clear ();
		}

	}

	public List<int> HandDeterminer(List<Card> hand){
		List<int> handRank = Pair (hand);
		return handRank;
	}

	private int HandComparer(){
		handRanking.Clear ()  ;
		dealerHandRank = HandDeterminer (dealerHand);
		handRanking.Clear ();
		playerOneHandRank = HandDeterminer (playerOneHand);		
		if(dealerHandRank[0] > playerOneHandRank[0]){
			winningPlayerNumber = 0;
		} else if(playerOneHandRank[0] > dealerHandRank[0]){
			winningPlayerNumber = 1;
		} 
		return winningPlayerNumber;
	}

	private List<int> FourOfAKind(List<Card> hand, bool twoPair, int pairValue, int threeOfAKindValue){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				for (int k = 0; k < 5; k++) {
					for (int l = 0; l < 5; l++){
						if(hand[i].Number == hand[j].Number && hand[j].Number == hand[k].Number 
							&& hand[k].Number == hand[l].Number  && i != j && i!= k && i!= l && j != k && j != l && k != l){
							handRanking.Add (7);
							handRanking.Add (hand[i].Number);
							return handRanking;
						}
					}
				}
			}
		}			
		if(twoPair){
			handRanking.Add (6);
			handRanking.Add (threeOfAKindValue);
			handRanking.Add (pairValue);
			return handRanking;
		}
		else{
			handRanking.Add (3);
			handRanking.Add (threeOfAKindValue);
			return handRanking;
		}

	}

	private List<int> Flush(List<Card> hand){
		if (hand[0].Suite == hand[1].Suite && hand[1].Suite == hand[2].Suite && hand[2].Suite 
			== hand[3].Suite && hand[3].Suite == hand[4].Suite){
			return Straight(hand, true);
		}
		else{
			return Straight(hand, false);
		}
	}

	private List<int> Straight(List<Card> hand, bool flush){
		List<int> cardNumbers = new List<int>();
		for(int i = 0; i < 5; i++){
			cardNumbers.Add (hand[i].Number);
		}
		cardNumbers.Sort ();
		int comparison = cardNumbers[0];
		if(cardNumbers[1] == comparison + 1 && cardNumbers[2] == comparison + 2 && cardNumbers [3] == comparison + 3 
			&& cardNumbers[4] == comparison + 4){
			if(flush){
				if(cardNumbers[4] == 12){
					handRanking.Add (9);
					return handRanking;
				}
				else{
					handRanking.Add (8);
					handRanking.Add (cardNumbers[4]);
					return handRanking;
				}
			}
			else{
				handRanking.Add (4);
				handRanking.Add (cardNumbers[4]);
				return handRanking;
			}
		}
		else if(flush){
			handRanking.Add (5);
			handRanking.Add (cardNumbers[4]);
			return handRanking;
		}
		else{
			handRanking.Add (0);
			return handRanking;
		}
	}

	private List<int> ThreeOfAKind(List<Card> hand, bool twoPair, int higherOrOnlyPairValue, int lowerPairValue){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				for (int k = 0; k < 5; k++) {
					if (hand[i].Number == hand[j].Number && hand[j].Number == hand[k].Number && i != j && j != k && i != k) {
						int pairValue = 0;
						if(hand[i].Number == higherOrOnlyPairValue){
							pairValue = lowerPairValue;
						}
						else{
							pairValue = higherOrOnlyPairValue;
						}
						return FourOfAKind (hand, twoPair, pairValue, hand[i].Number);
					}
				}
			}
		}
		if (twoPair){
			handRanking.Add (2);
			handRanking.Add (higherOrOnlyPairValue);
			handRanking.Add (lowerPairValue);
			return handRanking;
		}
		else{
			handRanking.Add (1);
			handRanking.Add (higherOrOnlyPairValue);
			return handRanking;
		}
	}

	private List<int> TwoPair(List<Card> hand, int pairValue){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				if(hand[i].Number == hand[j].Number && hand[i].Number != pairValue && i != j){
					if(pairValue > hand[i].Number){
						return ThreeOfAKind (hand, true, pairValue, hand [i].Number);
					}
					else{
						return ThreeOfAKind (hand, true, hand[i].Number, pairValue);
					}
				}
			}
		}	
		return ThreeOfAKind (hand, false, pairValue, 0);
	}

	private List<int> Pair(List<Card> hand){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				if (hand[i].Number == hand[j].Number && i != j) {
					return TwoPair (hand, hand[i].Number);
				}
			}
		}
		return Flush (hand);
	}
}

public class Card {
	public Card(int suite, int number, GameObject cardGraphic){
		Suite = suite;
		Number = number;
		CardGraphic = cardGraphic;
	}
	public int Suite { get; set; }
	public int Number { get; set; }
	public GameObject CardGraphic { get; set; }
}