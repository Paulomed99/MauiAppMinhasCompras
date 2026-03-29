using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;


public partial class ListaProduto : ContentPage
{
	
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();


	public ListaProduto()
	{

		try
		{

			InitializeComponent();

			lst_produtos.ItemsSource = lista;

		}
		catch (Exception ex)
		{

			DisplayAlert("Ops", ex.Message, "OK");


		}
	}
    protected async override void OnAppearing()
    {
		try
		{
			lista.Clear();

			List<Produto> tmp = await App.Db.GetAll();

			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex) {

			await DisplayAlert("Ops", ex.Message, "OK");
		
		}

		
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		} catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {

		try
		{
			string q = e.NewTextValue;

			lista.Clear();

			List<Produto> tmp = await App.Db.Search(q);

			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex) {

			await DisplayAlert("Ops", ex.Message, "OK");

		}

		
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {

		try
		{
			double soma = lista.Sum(i => i.Total);

			string msg = $"O total é {soma:C}";

			DisplayAlert("Total dos produtos", msg, "OK");
		}
		catch (Exception ex) {

			DisplayAlert("Ops", ex.Message, "OK");
		
		}

		
    }

    private async void SwipeItem_Invoked(object sender, EventArgs e)
    {
        try
        {
            SwipeItem swipeItem = sender as SwipeItem;
            Produto produtoSelecionado = swipeItem.BindingContext as Produto;

            if (produtoSelecionado != null)
            {
                bool confirmacao = await DisplayAlert("Atenção", $"Deseja realmente remover o produto '{produtoSelecionado.Descricao}'?", "Sim", "Não");

                if (confirmacao)
                {
                    await App.Db.Delete(produtoSelecionado.Id);
                    lista.Remove(produtoSelecionado);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
        {
            return;
        }

        try
        {
            // Pega o item clicado no formato do CollectionView
            Produto p = e.CurrentSelection.FirstOrDefault() as Produto;

            // Desmarca o item
            lst_produtos.SelectedItem = null;

            await Navigation.PushAsync(new Views.EditarProduto { BindingContext = p });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void SomarCategorias(object sender, EventArgs e)
    {
        try
        {
            if (lista.Count == 0)
            {
                DisplayAlert("Atenção", "Sua lista está vazia.", "OK");
                return;
            }

            var categoriasAgrupadas = lista.GroupBy(p => p.Categoria)
                                           .Select(grupo => new
                                           {
                                               NomeCategoria = grupo.Key,
                                               TotalGasto = grupo.Sum(p => p.Total)
                                           });

            string relatorio = "Resumo de Gastos:\n\n";

            foreach (var item in categoriasAgrupadas)
            {
                relatorio += $"{item.NomeCategoria}: {item.TotalGasto:C}\n";
            }

            DisplayAlert("Relatório por Categoria", relatorio, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void Filtrar_Clicked(object sender, EventArgs e)
    {
        try
        {
            DateTime inicio = dt_inicio.Date;
            DateTime fim = dt_fim.Date;

            if (inicio > fim)
            {
                await DisplayAlert("Atenção", "A data inicial não pode ser maior que a data final.", "OK");
                return;
            }

            List<Produto> produtosFiltrados = await App.Db.SearchData(inicio, fim);

            lista.Clear();
            produtosFiltrados.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void refresh_view_Refreshing(object sender, EventArgs e)
    {
        try
        {
            List<Produto> tmp = await App.Db.GetAll();

            lista.Clear();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            refresh_view.IsRefreshing = false;
        }
    }
}