using Godot;
using System;
using System.ComponentModel;

[GlobalClass]
public partial class CardData : Resource {
	[Export] public CardType type = CardType.Minion;
	[Export] public string name = "";
	[Export] public string description = "";
	[Export] public int cost = 1;
	[Export] public int health = 1;
	[Export] public int attack = 1;
	[Export] public Godot.Collections.Array<Abilities> abilities = new();
	[Export] public Godot.Collections.Array<Passives> passives = new();
	[Export] public AtlasTexture image = null;
}

