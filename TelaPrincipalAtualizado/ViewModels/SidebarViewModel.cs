using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TelaPrincipalAtualizado.Models;

namespace TelaPrincipalAtualizado.ViewModels
{
    public class SidebarViewModel : INotifyPropertyChanged
    {
        //rastrear o uitem selecionado
        private string _lastSelectedMenuItem;
        public string LastSelectedMenuItem
        {
            get => _lastSelectedMenuItem;
            set
            {
                if (_lastSelectedMenuItem != value)
                {
                    _lastSelectedMenuItem = value;
                    OnPropertyChanged();
                }
            }
        }

        // comando  que o XAML ira chamar quando clicar
        public ICommand ComandoNavegar { get; private set; }

        public SidebarViewModel()
        {
            ComandoNavegar = new Command<string>(ExecutarComandoNavegar);
        }
        private void ExecutarComandoNavegar(string nomeDoItem)
        {
            if (string.IsNullOrEmpty(nomeDoItem))
                return;

            LastSelectedMenuItem = nomeDoItem;

            MessagingCenter.Send(this, "NavegarPara", nomeDoItem);
        }

        //NotifyPropertyChanged

        //Evento de notificação que o xaml ouve
        public event PropertyChangedEventHandler PropertyChanged;

        //Método que dispara o evento
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
