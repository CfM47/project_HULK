﻿using AppInterface.ViewModel;

namespace AppInterface;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}

