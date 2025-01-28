using Godot;
using System;
using System.Collections.Generic;

public partial class GamePlayer : Node2D {
    [Signal] public delegate void OnDrawEventHandler(int hand_index);
    [Signal] public delegate void OnPlayEventHandler(int board_index);

    public int health = 25;
    public int mana = 0;
    public bool attacked = false;
    public GameHandler game_handler;

    public List<CardStatus> hand = new();
    public List<CardStatus> deck = new();
    public List<CardStatus> discard = new();

    public List<CardStatus> board = new();

    public GamePlayer(GameHandler game_handler, List<CardStatus> deck){
        this.game_handler = game_handler;
        this.deck = deck;
    }

    public CardStatus drawCard(int index = 0){
        if(deck.Count == 0){
            GD.Print("ran out of cards");
            return null;
        }
        CardStatus card = deck[index];
        deck.RemoveAt(index);
        hand.Add(card);
        EmitSignal("OnDraw", hand.Count-1);
        return card;
    }

    public void playCard(int id){
        game_handler.playMinion(id);
    }
}