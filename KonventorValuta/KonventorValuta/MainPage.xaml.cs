using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KonventorValuta
{
  
    public partial class MainPage : ContentPage
    {
        private const string urlAPI = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
        public IList<Valute> ListaValuta { get; private set; }
        public MainPage()
        {
            InitializeComponent();
            ListaValuta = new List<Valute>();
            ListaValuta.Add(new Valute
            {
                Naziv = "AUD",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/aud.png",
                Opis = "Australijski dolar"
            });
            ListaValuta.Add(new Valute 
            {
                Naziv = "BAM",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/bam.png",
                Opis = "Konvertibilna marka"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "CAD",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/cad.png",
                Opis = "Kanadski dolar"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "CNY",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/cny.png",
                Opis = "Kineski jan"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "CHF",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/chf.png",
                Opis = "Švicarski frank"

            });
            ListaValuta.Add(new Valute
            {
                Naziv = "CZK",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/czk.png",
                Opis = "Češka kruna"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "DKK",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/dkk.png",
                Opis = "Danska kruna"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "EUR",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/eur.png",
                Opis = "Euro"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "GBP",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/gbp.png",
                Opis = "Britanska funta"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "HRK",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/hrk.png",
                Opis = "Hrvatska kuna"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "HUF",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/huf.png",
                Opis = "Mađarska forinta"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "JPY",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/jpy.png",
                Opis = "Japanski jen"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "NOK",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/nok.png",
                Opis = "Norveška kruna"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "RSD",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/rsd.png",
                Opis = "Srbski dinar"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "SEK",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/sek.png",
                Opis = "Švedska kruna"
            });
            ListaValuta.Add(new Valute
            {
                Naziv = "USD",
                URL = "https://transferwise.com/public-resources/assets/flags/rectangle/usd.png",
                Opis = "Američki dolar"
            });
            BindingContext = this;

        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue)) return;
            if (!double.TryParse(e.NewTextValue, out double value))
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string sPrint = "";
            string rez = "";

            Valute odabir1 = new Valute();
            odabir1 = (Valute)ValutaOD.SelectedItem;

            Valute odabir2 = new Valute();
            odabir2 = (Valute)ValutaDO.SelectedItem;

            if (String.IsNullOrEmpty(Iznos.Text))
            {
                await DisplayAlert("Konventor valuta", "Iznos nije definisan!", "OK");
            }
            else if (ValutaOD.SelectedIndex == -1 || ValutaDO.SelectedIndex == -1)
            {
                await DisplayAlert("Konventor valuta", "Valuta nije odabrana!", "OK");
            }
            else
            {
                
                rez = Konventor(Convert.ToDecimal(Iznos.Text), odabir1.Naziv, odabir2.Naziv);

                sPrint = sPrint + $"Odabrani iznos: {Iznos.Text }  \n";
                sPrint = sPrint + $"Odabrana prva valuta: {odabir1.Naziv }  \n";
                sPrint = sPrint + $"Odabrana druga valuta: {odabir2.Naziv}  \n";
                sPrint = sPrint + $"Rezultat: {rez} \n";

                await DisplayAlert("Konventor valuta", sPrint, "OK");
            }
                       
        }       
        
        public string Konventor(decimal Iznos, string ValutaOD, string ValutaDO)
        {
            string url = string.Format(urlAPI, ValutaOD, ValutaDO);

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                decimal exRate = (decimal)token.SelectToken("rate");

                return (Iznos * exRate).ToString("#.000");
            }
        }

    }
           
}
