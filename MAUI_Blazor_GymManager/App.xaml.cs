﻿namespace MAUI_Blazor_GymManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "MAUI_Blazor_GymManager" };
        }
    }
}
