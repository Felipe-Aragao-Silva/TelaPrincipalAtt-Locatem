using System.Linq;
using TelaPrincipalAtualizado.Views;
using Microsoft.Maui.Controls;
using TelaPrincipalAtualizado.Models;
using System.Collections.ObjectModel;
using TelaPrincipalAtualizado.ViewModels;
using System.Threading.Tasks;


namespace TelaPrincipalAtualizado
{
    public partial class MainPage : ContentPage
    {
        //Variaveis de estado da sidebar
        private bool isSidebarOpen = true;
        private const double SidebarWidth = 300;

        private SidebarViewModel _sideBarViewModel;

        // Criar uma coleção observável de ferramentas. É uma lista que só aceita objetos do tipo Ferramenta
        public ObservableCollection<Ferramenta> Ferramentas { get; set; }


        public MainPage()
        {
            InitializeComponent();

            // lógica de conexão do ViewModel
            _sideBarViewModel = new SidebarViewModel();
            //Conecta o ViewModel apenas com a  sidebar
            sideBarLayout.BindingContext = _sideBarViewModel;


            //Para receber as ordens do clique
            MessagingCenter.Subscribe<SidebarViewModel, string>(this, "NavegarPara", async(sender, item) =>
                {
                await LidarComSolicitacaoDeNavegacao(item);
                });

            // Criar a lista de ferramentas
            Ferramentas = new ObservableCollection<Ferramenta>();

            var f1 = new Ferramenta(1, "Furadeira Elétrica", "Ferramentas Elétricas")
            {
                ImagemSource = "furadeira.png",
                PrecoDia = "R$ 25,00/dia",
                Disponibilidade = "Disponível"
            };
            Ferramentas.Add(f1);

            var f2 = new Ferramenta(2, "Serra Circular Bosch", "Ferramentas Elétricas")
            {
                ImagemSource = "serra.png",
                PrecoDia = "R$ 60,00/dia",
                Disponibilidade = "1 disponível"
            };
            Ferramentas.Add(f2);

            var f3 = new Ferramenta(3, "Lixadeira Orbital", "Ferramentas Elétricas")
            {
                ImagemSource = "lixadeira.png",
                PrecoDia = "R$ 40,00/dia",
                Disponibilidade = "Disponível"
            };
            Ferramentas.Add(f3);

            var f4 = new Ferramenta(4, "Parafusadeira", "Ferramentas Elétricas")
            {
                ImagemSource = "parafusadeira.png",
                PrecoDia = "R$ 30,00/dia",
                Disponibilidade = "2 disponíveis"
            };
            Ferramentas.Add(f4);

            //Abaixo aicionari ais ferramentas

            // Conecta a lista C# com o xaml, que é o famoso binding
            BindingContext = this;



        }

        // LÓGICA DE ABRIR E FECHAR A SIDEBAR
        private async void AoTocarMenu(object sender, EventArgs e)
        {
            //Pega a refêrencia á  primeira coluna (onde está a SideBar)
            var sidebarColumn = mainGrid.ColumnDefinitions[0];

            if (isSidebarOpen)
            {
                await sideBarLayout.TranslateTo(-SidebarWidth, 0, 250, Easing.CubicIn);
                sidebarColumn.Width = new GridLength(0);
                isSidebarOpen = false;
            }
            else
            {
                sidebarColumn.Width = new GridLength(SidebarWidth);
                await sideBarLayout.TranslateTo(0, 0, 250, Easing.CubicOut);
                isSidebarOpen = true;
            }
        }

        //===========================================
        // Lógica do clique dos items da sidebar
        //===========================================
        private async Task LidarComSolicitacaoDeNavegacao(string nomeDoItem)
        {
            Page proximaPagina = null;

            // ⚔️ Executor decide qual Jutsu de Navegação usar: Pop ou Push
            switch (nomeDoItem)
            {
                case "Inicio":
                    // Jutsu de Retorno: Se não estiver na raiz, desempilha.
                    if (Navigation.NavigationStack.Count > 1)
                    {
                        await Navigation.PopAsync();
                    }break;
                case "Carrinho":

                    break;
                case "Agendamento":
                    break;
                case "Histórico":
                    break;
                case "Notificações":
                    proximaPagina = new NotificacaoPage();
                    break;
                case "Suporte":
                    break;


            }

            // Executa o Push (Se for uma nova página)
            if (proximaPagina != null)
            {
                await Navigation.PushAsync(proximaPagina);
            }

            // Fecha a SideBar
            if (isSidebarOpen)
            {
                AoTocarMenu(this, EventArgs.Empty);
            }

        }
    }
}
