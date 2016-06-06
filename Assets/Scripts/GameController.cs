using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	//declaring public variables
	public GameObject[] deckGraphics;
	public Vector3[] cardGraphicsPositions;
	public Text winnerText;

	//declaring private variables
	private List<GameObject> playerOneHandGraphics = new List<GameObject> (5);
	private List<Card> deck = new List<Card>();
	private List<Card> playerOneHand = new List<Card> ();
	private List<Card> dealerHand = new List<Card> ();
	private List<Card> drawPile = new List<Card> ();
	//private List<int> cardNumbers = new List<int>();
	//private int[] dealerHandRank = new int[6];
	//private int[] playerOneHandRank = new int[6];
	//private int[] handRanking = new int[6];
	//private int numberOfCardsToDraw;
	private int dealButtonPressCount;
	private int playerNumber;
	private int cardNumber;
	private Vector3 cardGraphicsPosition;
	private Hand winningHand;

	//populates deck and drawPile
	private void Start(){
		winnerText.text = "";
		dealButtonPressCount = 0;
		for(int i = 0; i < 52; i++){
			int j = i % 13 + 2;
			int k = i / 13;
			string name = j.ToString ();
			if (i % 13 >= 9){
				switch (j){
				case 11:
					name = "Jack";
					break;
				case 12:
					name = "Queen";
					break;
				case 13:
					name = "King";
					break;
				case 14:
					name = "Ace";
					break;
				}
			}
			if (k == 0) {
				deck.Add (new Card(k, j, deckGraphics[i], "Clubs", name));
			} else if (k == 1) {
				deck.Add (new Card(k, j, deckGraphics[i], "Diamonds", name));
			} else if (k == 2){
				deck.Add (new Card(k, j, deckGraphics[i], "Hearts", name));
			} else if (k == 3){
				deck.Add (new Card(k, j, deckGraphics[i], "Spades", name));
			}
		}
		drawPile.AddRange (deck);
	}

	//called by DrawCard
	//picks number of random card from deck to deal
	private int CardNumber () {		
		return( Mathf.RoundToInt( Random.Range(0, drawPile.Count) ) );
	}

	//Called by DealButtonPress
	//Calls CardNumber
	//adds one random card to a specific player's hand
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

	//Called by user pressing the deal button
	//resets drawPile to match deck
	//calls DrawCard 5 times per player to populate each player's hand
	//lays out player's hand on the screen
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


	public void AnnounceWinner(){		
		string higherCardName = "";
		string lowerCardName = "";
		string winningHandName = "";
		int higherCard = winningHand.HandRank[1];
		int lowerCard = winningHand.HandRank[2];

		//converts card numbers to names for face cards to output correctly
		if (higherCard >= 11) {
			switch (higherCard) {
			case 11:
				higherCardName = "Jack";
				break;
			case 12:
				higherCardName = "Queen";
				break;
			case 13:
				higherCardName = "King";
				break;
			default:
				higherCardName = "Ace";
				break;
			}
		}
		if (lowerCard >= 11) {
			switch (lowerCard) {
			case 11:
				lowerCardName = "Jack";
				break;
			case 12:
				lowerCardName = "Queen";
				break;
			case 13:
				lowerCardName = "King";
				break;
			default:
				lowerCardName = "Ace";
				break;
			}
		}
		switch (winningHand.HandRank[0]){
		case 2:
			if (higherCard < 11) {
				winningHandName = "a pair of " + higherCard + "s!";
			} else {
				winningHandName = "a pair of " + higherCardName + "s!";
			}
			break;
		case 3:
			if (higherCard < 11) {
				if (lowerCard < 11) {
					winningHandName = "two pair " + higherCard + "s and " + lowerCard + "s!";
				} else {
					winningHandName = "two pair " + higherCard + "s and " + lowerCardName + "s!";
				}			
			} else {
				if (lowerCard < 11) {
					winningHandName = "two pair " + higherCardName + "s and " + lowerCard + "s!";
				} else {
					winningHandName = "two pair " + higherCardName + "s and " + lowerCardName + "s!";
				}			
			}
			break;
		case 4:
			if (higherCard < 11) {
				winningHandName = "three " + higherCard + "s!";
			} else {
				winningHandName = "three " + higherCardName + "s!";
			}
			break;
		case 5:
			if (higherCard < 11) {
				winningHandName = "a straight to the " + higherCard + "!";
			} else {
				winningHandName = "a straight to the " + higherCardName + "!";
			}
			break;
		case 6:
			winningHandName = "a flush!";
			break;
		case 7:
			if (higherCard < 11) {
				if (lowerCard < 11) {
					winningHandName = "a full house " + higherCard + "s over " + lowerCard + "s!";
				} else {
					winningHandName = "a full house " + higherCard + "s over " + lowerCardName + "s!";
				}			
			} else {
				if (lowerCard < 11) {
					winningHandName = "a full house " + higherCardName + "s over " + lowerCard + "s!";
				} else {
					winningHandName = "a full house " + higherCardName + "s over " + lowerCardName + "s!";
				}			
			}
			break;
		case 8:
			if (higherCard < 11) {
				winningHandName = "four " + higherCard + "s!";
			} else {
				winningHandName = "four " + higherCardName + "s!";
			}
			break;
		case 9:
			if (higherCard < 11) {
				winningHandName = "a straight flush to the " + higherCard + "!";
			} else {
				winningHandName = "a straight flush to the " + higherCardName + "!";
			}
			break;
		case 10:
			winningHandName = "a royal flush!";
			break;
		default:
			if (higherCard < 11){
				winningHandName = "a " + higherCard + " high!";
			}else{
				winningHandName = "a " + higherCardName + " high!";
			}
			break;
		}

		if (winningHand.WinningPlayer == "Tie"){
			switch (winningHand.HandRank[0]) { 
			case 0: //for high card hands
				if (higherCard < 11) {
					winnerText.text = "Player one and the dealer tie with " + higherCard + " high!";
				}
				break;
			}
		}else {
			winnerText.text = winningHand.WinningPlayer + " wins with " + winningHandName;
		}
	}

	//called by user pressing the replace button
	//calls HandComparer and AnnounceWinner
	public void ReplaceButtonPress(){
		HandComparer ();	
		AnnounceWinner ();
		for (int i = 0; i < winningHand.HandRank.Length; i++){
			Debug.Log (winningHand.HandRank[i]);
		}
	}

	//called by HandComparer
	//calls Pair starting the function tree that determines what rank the given hand is
	public int[] HandDeterminer(List<Card> hand){
		List<int> cardNumbers = new List<int> ();
		for(int i = 0; i < 5; i++){
			cardNumbers.Add (hand[i].Number);
		}
		cardNumbers.Sort ();
		cardNumbers.Reverse ();
		int[] handRank = Pair (hand, cardNumbers);
		return handRank;
	}

	//called by ReplacebuttonPress
	//calls HandDeterminer for each player's hand
	private void HandComparer(){
		int[] dealerHandRank = HandDeterminer (dealerHand);
		int[] playerOneHandRank = HandDeterminer (playerOneHand);
		//HandDeterminer (dealerHand).CopyTo (dealerHandRank, 0);
		//HandDeterminer (playerOneHand).CopyTo (playerOneHandRank, 0);
		for (int i = 0; i < dealerHandRank.Length; i++){
			if (dealerHandRank [i] > playerOneHandRank [i] && playerOneHandRank[i] != 0) {
				winningHand = new Hand (dealerHand, dealerHandRank, "Dealer");
				return;
			}else if (playerOneHandRank[i] > dealerHandRank[i] && playerOneHandRank[i] != 0){
				winningHand = new Hand (playerOneHand, playerOneHandRank, "Player One");
				return;
			}
		}
		winningHand = new Hand (dealerHand, dealerHandRank, "Tie");
		return;
	}

	private int[] FourOfAKind(List<Card> hand, List<int> cardNumbers, bool twoPair, int pairValue, int threeOfAKindValue){
		int[] handRanking = new int[6];
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				for (int k = 0; k < 5; k++) {
					for (int l = 0; l < 5; l++){
						if(hand[i].Number == hand[j].Number && hand[j].Number == hand[k].Number 
							&& hand[k].Number == hand[l].Number  && i != j && i!= k && i!= l && j != k && j != l && k != l){
							handRanking[0] = 8;
							handRanking[1] = hand[i].Number;
							if (hand[i].Number == cardNumbers[0]){
								handRanking[2] = cardNumbers[5];
							}else{
								handRanking[2] = cardNumbers[0];
							}
							return handRanking;
						}
					}
				}
			}
		}
		if(twoPair){
			handRanking[0] = 7;
			handRanking[1] = threeOfAKindValue;
			handRanking[2] = pairValue;
			return handRanking;
		}
		else{
			handRanking[0] = 4;
			handRanking[1] = threeOfAKindValue;
			int j = 2;
			for(int i = 0; i < 5; i++){
				if(cardNumbers[i] != threeOfAKindValue){
					handRanking[j] = cardNumbers[i];
					j++;
				}
			}
			return handRanking;
		}

	}

	private int[] Flush(List<Card> hand, List<int> cardNumbers){
		if (hand[0].SuiteInt == hand[1].SuiteInt && hand[1].SuiteInt == hand[2].SuiteInt && hand[2].SuiteInt 
			== hand[3].SuiteInt && hand[3].SuiteInt == hand[4].SuiteInt){
			return Straight(hand, cardNumbers, true);
		}
		else{
			return Straight(hand, cardNumbers, false);
		}
	}

	private int[] Straight(List<Card> handToEvaluate, List<int> cardNumbers, bool flush){
		int[] handRanking = new int[6];
		int comparison = cardNumbers[0];
		if(cardNumbers[1] == comparison - 1 && cardNumbers[2] == comparison - 2 && cardNumbers [3] == comparison - 3 
			&& cardNumbers[4] == comparison - 4){
			if(flush){
				if(cardNumbers[0] == 12){
					handRanking[0] = 10;
					handRanking[1] = 12;
					return handRanking;
				}
				else{
					handRanking[0] = 9;
					handRanking[1] = cardNumbers[0];
					return handRanking;
				}
			}
			else{
				handRanking[0] = 5;
				handRanking[1] = cardNumbers[0];
				return handRanking;
			}
		}
		else if(flush){
			handRanking[0] = 6;
			int j = 1;
			for (int i = 0; i < 5; i++){
				handRanking[j] = cardNumbers[i];
				j++;
			}			
			return handRanking;
		}
		else{
			handRanking[0] = 1;
			int j = 1;
			for (int i = 0; i < 5; i++){
				handRanking[j] = cardNumbers[i];
				j++;
			}			
			return handRanking;
		}
	}

	private int[] ThreeOfAKind(List<Card> handToEvaluate, List<int> cardNumbers, bool twoPair, int higherOrOnlyPairValue, int lowerPairValue){
		int[] handRanking = new int[6];
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				for (int k = 0; k < 5; k++) {
					if (cardNumbers[i] == cardNumbers[j] && cardNumbers[j] == cardNumbers[k] && i != j && j != k && i != k) {
						int pairValue = 0;
						if(cardNumbers[i] == higherOrOnlyPairValue){
							pairValue = lowerPairValue;
						}
						else{
							pairValue = higherOrOnlyPairValue;
						}
						return FourOfAKind (handToEvaluate, cardNumbers, twoPair, pairValue, handToEvaluate[i].Number);
					}
				}
			}
		}
		if (twoPair){
			handRanking[0] = 3;
			handRanking[1] = higherOrOnlyPairValue;
			handRanking[2] = lowerPairValue;
			for (int i = 0; i < 5; i++){
				if (cardNumbers[i] != higherOrOnlyPairValue && cardNumbers[i] != lowerPairValue){
					handRanking[3] = cardNumbers[i];
				}
			}
			return handRanking;
		}
		else{
			handRanking[0] = 2;
			handRanking[1] = higherOrOnlyPairValue;
			int j = 2;
			for (int i = 0; i < 5; i++){
				if (cardNumbers[i] != higherOrOnlyPairValue){
					handRanking[j] = cardNumbers[i];
					j++;
				}
			}
			return handRanking;
		}
	}

	private int[] TwoPair(List<Card> handToEvaluate, List<int> cardNumbers, int pairValue){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				if(cardNumbers[i] == cardNumbers[j] && cardNumbers[i] != pairValue && i != j){
					return ThreeOfAKind (handToEvaluate, cardNumbers, true, pairValue, cardNumbers[i]);
				
				}
			}
		}	
		return ThreeOfAKind (handToEvaluate, cardNumbers, false, pairValue, 0);
	}

	private int[] Pair(List<Card> handToEvaluate, List<int> cardNumbers){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				if (cardNumbers[i] == cardNumbers[j] && i != j){
					return TwoPair (handToEvaluate, cardNumbers, cardNumbers[j]);
				}
			}
		}
		return Flush (handToEvaluate, cardNumbers);
	}
}

public class Card {
	public Card(int suiteInt, int number, GameObject cardGraphic, string suite, string name){
		SuiteInt = suiteInt;
		Number = number;
		CardGraphic = cardGraphic;

	}
	public int SuiteInt { get; set; }
	public int Number { get; set; }
	public GameObject CardGraphic { get; set; }		
	public string Suite { get; set; }
	public string Name { get; set; }
}

public class Hand{
	public Hand(List<Card> cards, int[] handRank, string winningPlayer){
		Cards = cards;
		HandRank = handRank;
		WinningPlayer = winningPlayer;
	}
	public List<Card> Cards { get; set; }
	public int[] HandRank { get; set; }
	public string WinningPlayer { get; set; }
}