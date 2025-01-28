using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class GameHandler : Node2D
{
	private const string card_data_path = "res://resources/card_data";
	private const string minion_scene_path = "res://scenes/minion.tscn";

	public GamePlayer player1;
	public GamePlayer player2;

	[Export] public UIController ui;

	private int card_id = 1;
	private int turn = 0; // even number is player 1 turn, vise versa
	private bool attacked = false;

	private Random random = new Random();

	private List<CardStatus> shuffle(List<CardStatus> deck){  
		int n = deck.Count;
		while(n-- > 1){
			int k = random.Next(n+1);
			CardStatus placeholder = deck[k];
			deck[k] = deck[n];
			deck[n] = placeholder;
		}
		return deck;
	}
	
	public async void startGame(List<string> deck1, List<string> deck2){
		List<CardStatus> actual_deck1 = new();
		List<CardStatus> actual_deck2 = new();
		deck1.ForEach(name => {
			CardData data = ResourceLoader.Load<CardData>($"{card_data_path}/{name}.tres");
			actual_deck1.Add(new CardStatus(card_id++, data));
		});
		deck2.ForEach(name => {
			CardData data = ResourceLoader.Load<CardData>($"{card_data_path}/{name}.tres");
			actual_deck2.Add(new CardStatus(card_id++, data));
		});
		player1 = new GamePlayer(this, shuffle(actual_deck1));
		player2 = new GamePlayer(this, shuffle(actual_deck2));

		ui.linkPlayer();

		for(int i = 0; i < 5; i++){
			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
			player1.drawCard();
			player2.drawCard();
		}
	}

	public void playMinion(int id){
		GD.Print("card with id " + id + " played");
	}

	public void nextTurn(){
		player1.drawCard();
		player2.drawCard();
	}

    public override void _Ready()
    {
        GetNode<Button>("/root/Game/CanvasLayer/Button").Pressed += nextTurn;
    }

}
