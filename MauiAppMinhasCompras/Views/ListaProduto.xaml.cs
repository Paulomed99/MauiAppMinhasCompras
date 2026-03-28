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

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			MenuItem menuItem = sender as MenuItem;

			Produto produtoSelecionado = menuItem.BindingContext as Produto;

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
		catch(Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
		try
		{

			Produto p = e.SelectedItem as Produto;

			Navigation.PushAsync(new Views.EditarProduto { BindingContext = p });
				
		}

		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message , "OK");
		}

    }

    private void SomarCategorias(object sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);

            string msg = $"O total é {soma:C}";

            DisplayAlert("Total dos produtos", msg, "OK");
        }
        catch (Exception ex)
        {

            DisplayAlert("Ops", ex.Message, "OK");

        }
    }
}