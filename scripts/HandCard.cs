using Godot;
using System;
using System.Collections.Generic;

public partial class HandCard : Control
{
    CardStatus stats;
    bool is_being_dragged = false;
    bool is_hovering = false;

    public override void _Ready(){
	}

	public override void _Process(double delta){

	}
 
    public override void _Input(InputEvent @event)
    {
        
    }
}
