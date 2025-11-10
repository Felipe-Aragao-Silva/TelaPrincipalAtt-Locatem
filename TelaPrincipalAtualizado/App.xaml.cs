using Microsoft.Maui.Controls;
using TelaPrincipalAtualizado.Views;

namespace TelaPrincipalAtualizado
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}