using System.Linq;
using TelaPrincipalAtualizado.Views;
using Microsoft.Maui.Controls;
using TelaPrincipalAtualizado.Models;
using System.Collections.ObjectModel;


namespace TelaPrincipalAtualizado
{
    public partial class MainPage : ContentPage
    {

        //Essa variável de estado é para rastrear se a sdidebar está aberta ou fechada
        private bool isSidebarOpen = true;

        //Constante da largura da SideBar, conforme foi definido no XAML
        private const double SidebarWidth = 300;

        // Rastreia qual  item do menu foi clicado por ultimo
        private Grid _lastSelectedMenuItem;

        // Criar uma coleção observável de ferramentas. É uma lista que só aceita objetos do tipo Ferramenta
        public ObservableCollection<Ferramenta> Ferramentas { get; set; }


        public MainPage()
        {
            InitializeComponent();



            //Conecta os pontos de clique dos botões (IndicadorView) ao Banner (CarrouselView)
            BannerCarousel.IndicatorView = BannerIndicator;


            

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
        private async void OnMenuTapped(object sender, EventArgs e)
        {
            //Pega a refêrencia á  primeira coluna (onde está a SideBar)
            var sidebarColumn = mainGrid.ColumnDefinitions[0];

            if (isSidebarOpen)
            {
                //=================================================================
                // LÓGICA PARA FECHAR A SIDEBAR ABERTA
                //O conteúdo principal redimensiona e a coluna 'meio que some'
                //=================================================================

                //Animação : Desliza o layout da sidebar para fora da tela
                // O await garante que a animaçao termine antes de reduzir a coluna
                await sideBarLayout.TranslateTo(-SidebarWidth, 0, 250, Easing.CubicIn);

                //Redimensionamento : Reduz a largura da coluna da sidebar para zero
                //Isso faz com que o conteudo principal ocupe toda a tela
                // e é oq gera o efeito aumentar
                sidebarColumn.Width = new GridLength(0);

                isSidebarOpen = false;
            }
            else 
            { 
                //=================================================================
                // LÓGICA PARA ABRIR A SIDEBAR FECHADA
                //O conteúdo principal redimensiona e a coluna 'meio que aparece'
                //=================================================================
                
                //Redimensionamento : Define a largura da coluna da sidebar para o valor original
                sidebarColumn.Width = new GridLength(SidebarWidth);
                
                //Animação : Desliza o layout da sidebar para dentro da tela
                await sideBarLayout.TranslateTo(0, 0, 250, Easing.CubicOut);

                isSidebarOpen = true;
            }
        }

        //===========================================
        // Lógica do clique dos items da sidebar
        //===========================================
        private async void OnMenuItemTapped(object sender, EventArgs e) 
        {
            // Identifica qual grid foi clicado
            var clickedGrid = sender as Grid;

            // Extrai o nome do item clicado usando o CommandParameter que eu defini no XAML
            if (clickedGrid?.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tapGesture) 
            {
                //pega o valor do: inicio e assim por diante
                string menuItem = tapGesture.CommandParameter.ToString();
                System.Diagnostics.Debug.WriteLine($"Menu clicado{menuItem}");

                Page nextPage = null;

                // Aqui vai fica a lógica para muda o estado visual do botão
                if (_lastSelectedMenuItem != null) 
                {
                    //Ele vai pegar o itwm anterior e definir o seu estado visual como 'Normal' sem a cor de fundo
                    VisualStateManager.GoToState(_lastSelectedMenuItem, "Normal");
                }

                // Define o estado visual do item recem clicado como 'Selecionado', que tem a cor de fundo diferente (cor cinza)
                VisualStateManager.GoToState(clickedGrid, "Selecionado");

                //E por ultimo atualiza a variável que rastreia o ultimo item clicado
                // E tambem guarda refêrencia do Grid atual  para que possamos desselecioná-los no próximo clique
                _lastSelectedMenuItem = clickedGrid;

                // Aqui eu vou adicionar a lógica para navegar para diferentes páginas 


                switch (menuItem) 
                {
                    case "Inicio":
                        // Navegar para a página inicial
                        nextPage = new MainPage();
                        break;
                    case "Carrinho":
                        // Navegar para a página de carrinho
                        System.Diagnostics.Debug.WriteLine("Navegando para o Carrinho");
                        break;
                    case "Agendamento":
                        // Navegar para a página de agendamento
                        System.Diagnostics.Debug.WriteLine("Navegando para o Agendamento");
                        break;
                    case "Histórico":
                        // Navegar para a página de histórico
                        System.Diagnostics.Debug.WriteLine("Navegando para o Histórico");
                        break;
                    case "Notificações":
                        // Navegar para a página de notificações
                        nextPage = new NotificacaoPage();
                        break;
                    case "Suporte":
                        // Navegar para a página de suporte
                        System.Diagnostics.Debug.WriteLine("Navegando para o Suporte");
                        break;

                }

                // 3. Executa a navegação e fecha a SideBar
                if (nextPage != null)
                {
                    // Navega para a nova página
                    await Navigation.PushAsync(nextPage);

                    // Fecha a SideBar após a navegação (reutiliza a lógica OnMenuTapped)
                    // Checa se está aberta antes de tentar fechar.
                    if (isSidebarOpen)
                    {
                        OnMenuTapped(null, EventArgs.Empty);
                    }
                }
                else if (menuItem == "Início")
                {
                    // Garante que a SideBar feche mesmo se não houver navegação (ex: clicou em 'Início' na Home Page)
                    if (isSidebarOpen)
                    {
                        OnMenuTapped(null, EventArgs.Empty);
                    }
                }
            }
        }

    }
}
