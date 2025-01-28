using Godot;
using System;
using System.Collections.Generic;

public partial class coolTester : Node2D
{
    public override void _Ready()
    {
        GD.Print("press enter plz");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Enter){
            GetNode<GameHandler>("/root/Game").startGame(
                new List<string>(){"Bob","Bob","Bob","Bob","Bob","Bob","Bob","Bob","Bob","Bob"},
                new List<string>(){"Bob","Bob","Bob","Bob","Bob","Bob","Bob","Bob","Bob","Bob"}
            );
        }
    }
}