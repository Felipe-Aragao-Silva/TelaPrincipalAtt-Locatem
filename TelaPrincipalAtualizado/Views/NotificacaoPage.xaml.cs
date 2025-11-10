using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using TelaPrincipalAtualizado.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TelaPrincipalAtualizado.Views;

public partial class NotificacaoPage : ContentPage
{
    private List<Notificacao> todasNotificacoes;
    private string filtroAtual = "TODAS";

    public NotificacaoPage()
	{
		InitializeComponent();
        CarregarNotificacoes();
        MostrarNotificacoes(todasNotificacoes);
    }

    private void OnMenuClicked(object sender, EventArgs e)
    {
        PainelLateral.IsVisible = !PainelLateral.IsVisible;
        MainGrid.ColumnDefinitions[0].Width = PainelLateral.IsVisible ? new GridLength(300) : 0;
    }

    private void CarregarNotificacoes()
    {
        todasNotificacoes = new List<Notificacao>
            {
                // Hoje
                new("Furadeira Bosch", "Reserva confirmada", DateTime.Now),
                new("Betoneira 400L", "Devolução em atraso", DateTime.Now.AddHours(-2)),

                // Últimos 7 dias
                new("Gerador 5kVA", "Devolução agendada", DateTime.Now.AddDays(-1)),
                new("Compressor de Ar 24L", "Aguardando manutenção", DateTime.Now.AddDays(-3)),
                new("Martelete 10kg", "Renovação efetuada", DateTime.Now.AddDays(-5)),

                // Complemento do filtro "Todas"
                new("Serra Circular Makita", "Aguardando devolução", DateTime.Now.AddDays(-6)),
                new("Escada Extensível 10m", "Uso prolongado", DateTime.Now.AddDays(-4)),
                new("Torre de Iluminação", "Locação finalizada", DateTime.Now.AddDays(-10)),
                new("Parafusadeira Elétrica", "Pagamento confirmado", DateTime.Now.AddDays(-15)),
                new("Andaime Suspenso", "Limpeza necessária", DateTime.Now.AddDays(-20))
            };
    }

    private void MostrarNotificacoes(IEnumerable<Notificacao> notificacoes)
    {
        NotificationsStack.Children.Clear();

        foreach (var n in notificacoes)
        {
            var card = new Border
            {
                Stroke = (Color)Application.Current.Resources["CorPadrao"],
                StrokeThickness = 1,
                BackgroundColor = Colors.White,
                Padding = 15,
                Margin = new Thickness(0, 0, 0, 10),
                StrokeShape = new RoundRectangle { CornerRadius = 10 }
            };

            var content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Auto)
                    }
            };

            content.Add(new VerticalStackLayout
            {
                Children =
                    {
                        new Label { Text = n.Ferramenta, FontAttributes = FontAttributes.Bold, FontSize = 18, TextColor = Colors.Black },
                        new Label { Text = $"{n.Status} - {n.Data:dd/MM/yyyy HH:mm}", FontSize = 14, TextColor = Colors.Gray }
                    }
            });

            var detailsButton = new Button
            {
                Text = "Ver detalhes",
                BackgroundColor = (Color)Application.Current.Resources["CorPadrao"],
                TextColor = (Color)Application.Current.Resources["Detalhe"],
                CornerRadius = 8,
                Padding = new Thickness(10, 5),
                Command = new Command(() => MostrarDetalhes(n))
            };

            content.Add(detailsButton, 1, 0);
            card.Content = content;
            NotificationsStack.Children.Add(card);
        }
    }

    private async void MostrarDetalhes(Notificacao n)
    {
        string formaPagamento = "", entrega = "";
        decimal valor = 0;
        int dias = 0;
        string novoStatus = n.Status;

        switch (n.Ferramenta)
        {
            case "Furadeira Bosch":
                novoStatus = "Reserva confirmada";
                formaPagamento = "Pix";
                entrega = "Separando equipamentos";
                valor = 120;
                dias = 2;
                break;
            case "Betoneira 400L":
                novoStatus = "Devolução em atraso";
                formaPagamento = "Dinheiro";
                entrega = "Aguardando devolução";
                valor = 180;
                dias = 4;
                break;
            case "Gerador 5kVA":
                novoStatus = "Devolução agendada";
                formaPagamento = "Cartão de crédito";
                entrega = "Devolução agendada para amanhã";
                valor = 250;
                dias = 5;
                break;
            case "Compressor de Ar 24L":
                novoStatus = "Aguardando manutenção";
                formaPagamento = "Boleto bancário";
                entrega = "Manutenção";
                valor = 200;
                dias = 3;
                break;
            case "Martelete 10kg":
                novoStatus = "Renovação efetuada";
                formaPagamento = "Depósito";
                entrega = "Data atualizada";
                valor = 300;
                dias = 2;
                break;
            case "Serra Circular Makita":
                novoStatus = "Aguardando devolução";
                formaPagamento = "Pix";
                entrega = "Devolução agendada para: HOJE";
                valor = 110;
                dias = 2;
                break;
            case "Escada Extensível 10m":
                novoStatus = "Uso prolongado";
                formaPagamento = "Transferência bancária";
                entrega = "Uso prolongado, verifique necessidade de renovação";
                valor = 80;
                dias = 7;
                break;
            case "Torre de Iluminação":
                novoStatus = "Locação finalizada";
                formaPagamento = "Transferência bancária";
                entrega = "Locação finalizada";
                valor = 320;
                dias = 7;
                break;
            case "Parafusadeira Elétrica":
                novoStatus = "Pagamento confirmado";
                formaPagamento = "Cartão de crédito";
                entrega = "Pagamento confirmado";
                valor = 60;
                dias = 1;
                break;
            case "Andaime Suspenso":
                novoStatus = "Limpeza necessária";
                formaPagamento = "Depósito";
                entrega = "Limpeza do item";
                valor = 400;
                dias = 10;
                break;
        }

        // Atualiza o Status na lista principal
        int index = todasNotificacoes.FindIndex(x => x.Ferramenta == n.Ferramenta);
        if (index >= 0)
            todasNotificacoes[index] = n with { Status = novoStatus };

        // Reaplica o filtro atual
        switch (filtroAtual)
        {
            case "HOJE":
                MostrarNotificacoes(todasNotificacoes.Where(x => x.Data.Date == DateTime.Today));
                break;
            case "7DIAS":
                MostrarNotificacoes(todasNotificacoes.Where(x => x.Data >= DateTime.Now.AddDays(-7) && x.Data.Date < DateTime.Today));
                break;
            default:
                MostrarNotificacoes(todasNotificacoes);
                break;
        }

        // Mostra popup de detalhes
        await DisplayAlert("Detalhes da Locação",
            $"Ferramenta: {n.Ferramenta}\n" +
            $"Status: {novoStatus}\n" +
            $"Data: {n.Data:dd/MM/yyyy HH:mm}\n" +
            $"Dias de locação: {dias}\n" +
            $"Valor: R$ {valor:0.00}\n" +
            $"Forma de pagamento: {formaPagamento}\n" +
            $"Entrega: {entrega}\n",
            "Fechar");
    }

    private async void OnFilterClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Filtrar Notificações", "Fechar", null, "HOJE", "ÚLTIMOS 7 DIAS", "TODAS AS NOTIFICAÇÕES");
        if (action == null || action == "Fechar") return;

        switch (action)
        {
            case "HOJE":
                filtroAtual = "HOJE";
                MostrarNotificacoes(todasNotificacoes.Where(n => n.Data.Date == DateTime.Today));
                break;
            case "ÚLTIMOS 7 DIAS":
                filtroAtual = "7DIAS";
                MostrarNotificacoes(todasNotificacoes.Where(n => n.Data >= DateTime.Now.AddDays(-7) && n.Data.Date < DateTime.Today));
                break;
            case "TODAS AS NOTIFICAÇÕES":
                filtroAtual = "TODAS";
                MostrarNotificacoes(todasNotificacoes);
                break;
        }
    }

    private async void OnClearAllClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmação",
            "Tem certeza que deseja limpar todas as notificações?",
            "Sim", "Cancelar");

        if (confirm)
        {
            NotificationsStack.Children.Clear();
            await DisplayAlert("Notificações", "Todas as notificações foram limpas.", "OK");
        }
    }

}