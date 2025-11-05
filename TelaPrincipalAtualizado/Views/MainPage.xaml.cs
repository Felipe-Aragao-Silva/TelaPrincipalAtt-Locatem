//Usar o molde ferramenta
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

            //Aqui cadiciona as ferramentas nos retangulos, quer dizer cria os retangulos

            // Criar a lista de ferramentas
            Ferramentas = new ObservableCollection<Ferramenta>();

            Ferramentas.Add(new Ferramenta
            {
                Titulo = "Furadeira Elétrica",
                ImagemSource = "furadeira.png",
                PrecoDia = "R$ 25,00/dia",
                Disponibilidade = "Disponível"
            });

            Ferramentas.Add(new Ferramenta
            {
                Titulo = "Serra Circular Bosch",
                ImagemSource = "serra.png",
                PrecoDia = "R$ 60,00/dia",
                Disponibilidade = "1 disponível"
            });

            //Abaixo aicionari ais ferramentas

            // Conecta a lista C# com o xaml, que é o famoso binding
            BindingContext = this;

            //Conecta os pontos de clique dos botões (IndicadorView) ao Banner (CarrouselView)
            BannerCarousel.IndicatorView = BannerIndicator;
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

        // LÓGICA DA nAVEGAÇÃO DO BANNER
        private void OnPrevBannerClicked(object sender, EventArgs e)
        {
            // Obtem o indice atual do banner, resumindo pega o banner atual
            int currentIndex = BannerCarousel.Position;

            //Calcula o indice do banner anterior. Se estiver no primeiro banner, volta para o ultimo
            int previousIndex = (currentIndex == 0)
                ? BannerCarousel.ItemsSource.Cast<object>().Count() - 1
                : currentIndex - 1;

            //Definme a nova posição do Banner
            BannerCarousel.Position = previousIndex;
        }

        private void OnNextBannerClicked(object sender, EventArgs e)
        {
            //Obtem o número total de itens no Banner
            int totalItems = BannerCarousel.ItemsSource.Cast<object>().Count();

            //Obtem o indice atual do banner
            int currentIndex = BannerCarousel.Position;

            //calcula o proximo indice. se estiver no ultimo banner, volta para o primeiro
            int nextIndex = (currentIndex == totalItems - 1)
                ? 0
                : currentIndex + 1;

            //Define a nova posição do Banner
            BannerCarousel.Position = nextIndex;
        }







        // Lógica do clique dos items da sidebar

        private void OnMenuItemTapped(object sender, EventArgs e) 
        {
            // Identifica qual grid foi clicado
            var clickedGrid = sender as Grid;

            // Extrai o nome do item clicado usando o CommandParameter que eu defini no XAML
            if (clickedGrid?.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tapGesture) 
            {
                //pega o valor do: inicio e assim por diante
                string menuItem = tapGesture.CommandParameter.ToString();
                System.Diagnostics.Debug.WriteLine($"Menu clicado{menuItem}");

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
                        System.Diagnostics.Debug.WriteLine("Navegando para o Início");
                        break;
                    case "Carrinho":
                        // Navegar para a página de categorias
                        System.Diagnostics.Debug.WriteLine("Navegando para o Carrinho");
                        break;
                    case "Agendamento":
                        // Navegar para a página de agenda
                        System.Diagnostics.Debug.WriteLine("Navegando para a Agendamento");
                        break;
                    case "Histórico":
                        // Navegar para a página de histórico
                        System.Diagnostics.Debug.WriteLine("Navegando para o Histórico");
                        break;
                    case "Notificações":
                        // Navegar para a página de perfil
                        System.Diagnostics.Debug.WriteLine("Navegando para as Notificações");
                        break;
                    case "Suporte":
                        // Navegar para a página de configurações
                        System.Diagnostics.Debug.WriteLine("Navegando para o Suporte");
                        break;

                }
            }
        }




    }
}
