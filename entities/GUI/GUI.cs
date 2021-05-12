using Godot;
using System;

public class GUI : MarginContainer
{
    private Label _healthNumber;

    public override void _Ready()
    {
        _healthNumber = GetNode<Label>("Bars/HealthBar/HealthCount/Container/Number");
    }

    public void SetHealth(int value) {
        _healthNumber.Text = value.ToString();
    }
}
