using Godot;
using System;

public class Level1 : Node
{
    private GUI _gui;
    private Hero _hero;

    public override void _Ready()
    {
        _gui = GetNode<GUI>("CanvasLayer/GUI");
        _hero = GetNode<Hero>("Hero");
        _hero.Connect(nameof(Hero.HealthChanged), _gui, nameof(GUI.SetHealth));
    }
}
