using Godot;
using System;
using System.Collections.Generic;

public partial class UIController : Node2D
{
    private const string card_data_path = "res://resources/card_data";
	private const string ui_card = "res://scenes/card.tscn";

	private List<Control> ui_cards = new();
    private Control ui_hand;

    private Control hover_card;
    private Control drag_card;
    private bool dragging;

    private GamePlayer plr;
    private GameHandler gameHandler;

    public void addCard(int index){
        CardStatus card_stats = gameHandler.player1.hand[index];
        CardData base_stats = ResourceLoader.Load<CardData>($"{card_data_path}/{card_stats.name}.tres");

        PackedScene loaded_card = ResourceLoader.Load<PackedScene>(ui_card);

        Control clone = loaded_card.Instantiate() as Control;
        clone.GetNode<RichTextLabel>("NameLabel").Text = base_stats.name;
        clone.GetNode<RichTextLabel>("DescLabel").Text = base_stats.description;
        clone.GetNode<RichTextLabel>("AttackLabel").Text = card_stats.attack.ToString();
        clone.GetNode<RichTextLabel>("HealthLabel").Text = card_stats.health.ToString();
        clone.GetNode<RichTextLabel>("CostLabel").Text = card_stats.cost.ToString();
        clone.GetNode<TextureRect>("ImageLabel").Texture = base_stats.image;
        GetNode<Control>("/root/Game/CanvasLayer/Hand").AddChild(clone);
        clone.Position = new Vector2(1600,800);
        ui_cards.Add(clone);

        clone.MouseEntered += () => onHover(clone);
        clone.MouseExited += () => onHoverExit(clone);

        updateCardPosition();
    }

    public void linkPlayer(){
        plr = gameHandler.player1;
        plr.DrawCard += addCard;
    }

	public void updateCardPosition(){
        int count = ui_cards.Count;
        int side_count = count/2;
        int hover_index = hover_card != null ? ui_cards.IndexOf(hover_card) : side_count;

        for (int i = 0; i < ui_cards.Count; i++){
            Control card = ui_cards[i];

            float x;
            float y;

            float angle = 0;
            Vector2 scale = new Vector2(1.5f,1.5f);

            if(card == hover_card && !dragging){
                x = 950 + (side_count-i)*-165;
                card.ZIndex = 1;
                scale = new Vector2(2.75f,2.75f);
                y = 550;
            } else {
                float coef = hover_index-i;
                x = 1000 + (side_count-i) * -165 + (hover_index-i)*(float)Math.Pow(coef,2)/10;
                card.ZIndex = 0;
                y = 875+Math.Abs(side_count-i)*10;
                angle = (side_count-i)*-3.5f;
            }

            Vector2 pos = new Vector2(x,y);

            Tween tween = CreateTween();

            tween.TweenProperty(card, "position", pos, 0.2f)
                .SetTrans(Tween.TransitionType.Quart);

            tween.Parallel().TweenProperty(card, "rotation_degrees", angle, 0.2f)
                .SetTrans(Tween.TransitionType.Quart);

            tween.Parallel().TweenProperty(card, "scale", scale, 0.1f)
                .SetTrans(Tween.TransitionType.Quart);
        }
	}

    public void onHover(Control card){
        if(hover_card == null && !dragging){
            hover_card = card;
            updateCardPosition();
        }
    }

    public void onHoverExit(Control card){
        if(hover_card == card && !dragging){
            hover_card = null;
            updateCardPosition();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left){
            if(click.IsPressed()){
                if(hover_card != null){
                    drag_card = hover_card.Duplicate() as Control;
                    ui_hand.GetParent().AddChild(drag_card);

                    dragging = true;
                    hover_card.Visible = false;

                    Tween tween = CreateTween();
                    tween.Parallel().TweenProperty(drag_card, "rotation_degrees", 0f, 0.2f)
                        .SetTrans(Tween.TransitionType.Linear);

                    tween.Parallel().TweenProperty(drag_card, "scale", new Vector2(1.5f,1.5f), 0.1f)
                        .SetTrans(Tween.TransitionType.Linear);
                }
            } else {
                if(dragging){
                    hover_card.Visible = true;
                    hover_card = null;
                    dragging = false;
                    drag_card.QueueFree();
                }
                updateCardPosition();
            }
        }
    }

    public override void _Ready(){
        gameHandler = GetNode<GameHandler>("/root/Game");
        ui_hand = GetNode<Control>("/root/Game/CanvasLayer/Hand");
	}

    public override void _Process(double delta){
        if(dragging){
            Vector2 pos = GetGlobalMousePosition();
            GD.Print(drag_card.Size);
            pos.X -= drag_card.Size.X;
            pos.Y -= drag_card.Size.Y;
            drag_card.Position = pos;
        }
	}
}
