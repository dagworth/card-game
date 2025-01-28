using Godot;
using System;

public class CardStatus {
    public CardType type = CardType.Minion;
    public Control hover_card;
    public string name = "";
    public int cost = 1;
    public int health = 1;
    public int max_health = 1;
    public int attack = 1;
    public Godot.Collections.Array<Abilities> abilities = new();
    public Godot.Collections.Array<Passives> passives = new();

    public CardStatus(CardData data){
        type = data.type;
        name = data.name;
        cost = data.cost;
        health = data.health;
        max_health = data.health;
        attack = data.attack;
        abilities = data.abilities;
        passives = data.passives;
        createHover();
    }

    public void createHover(){

    }
}
