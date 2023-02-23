using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace projektarbeit_Pizzashop
{
    /// <summary>
    /// Interaktionslogik für BestellungView.xaml
    /// </summary>
    public partial class BestellungView : Window
    {
        BestellungController bestellungController;
        KundenController kundenController;
        PizzaController pizzaController;
        List<Bestellposition> bestellpositionen;
        //Extrazutaten, die von der Auswahl auf die Pizza gepackt werden
        List<Zutat> extrazutatenAufPizza;
        //Extrazutaten, die zur Auswahl stehen
        List<Zutat> zutaten;
        List<Kunde> kunden;
        List<Pizza> pizzas;
        //Suche
        List<Bestellung> bestellungenFiltered;
        private bool edited;

        public BestellungView()
        {
            InitializeComponent();

            bestellungController = new BestellungController();
            kundenController = new KundenController();
            pizzaController = new PizzaController();
            bestellpositionen = new List<Bestellposition>();
            extrazutatenAufPizza = new List<Zutat>();
            zutaten = new List<Zutat>();
            bestellungenFiltered = new List<Bestellung>();
            edited = false;

            kunden = kundenController.ReadKunde();
            pizzas = pizzaController.ReadPizza();

            foreach (Kunde kunde in kunden)
            {
                coxKunde.Items.Add($"{kunde.Nachname} {kunde.Vorname} {kunde.Geburtsdatum}");
            }
            foreach (Pizza pizza in pizzas)
            {
                coxPizza.Items.Add($"{pizza.Name} {pizza.Einzelpreis} CHF");
            }

            //Zutaten Presets
            Zutat tomatensosse = new Zutat("Tomatensosse", 5);
            Zutat mozzarella = new Zutat("Mozzarella", 5);
            Zutat salami = new Zutat("Salami", 5);
            Zutat schinken = new Zutat("Schinken", 5);
            Zutat meeresfruechte = new Zutat("Meeresfruechte", 5);
            Zutat zwiebeln = new Zutat("Zwiebeln", 5);
            Zutat ananas = new Zutat("Ananas", 100);
            Zutat pilze = new Zutat("Pilze", 5);
            Zutat oregano = new Zutat("Oregano", 5);
            Zutat peperoni = new Zutat("Peperoni", 5);
            Zutat oliven = new Zutat("Oliven", 5);
            Zutat sardellen = new Zutat("Sardellen", 5);
            Zutat kapern = new Zutat("Kapern", 5);
            Zutat gorgonzola = new Zutat("Gorgonzola", 5);
            Zutat rucola = new Zutat("Rucola", 5);
            Zutat tomaten = new Zutat("Tomaten", 5);
            Zutat broccoli = new Zutat("Brocoli", 5);
            Zutat artischocken = new Zutat("Artischocken", 5);
            Zutat schokolade = new Zutat("Schokolade", 5);
            Zutat scharfeSalami = new Zutat("Scharfe Salami", 5);
            Zutat knoblauch = new Zutat("Knoblauch", 5);
            Zutat thon = new Zutat("Thon", 5);
            Zutat rohschinken = new Zutat("Rohschinken", 5);
            Zutat speck = new Zutat("Speck", 5);

            zutaten.Add(tomatensosse);
            zutaten.Add(mozzarella);
            zutaten.Add(salami);
            zutaten.Add(schinken);
            zutaten.Add(meeresfruechte);
            zutaten.Add(zwiebeln);
            zutaten.Add(ananas);
            zutaten.Add(pilze);
            zutaten.Add(oregano);
            zutaten.Add(peperoni);
            zutaten.Add(oliven);
            zutaten.Add(sardellen);
            zutaten.Add(kapern);
            zutaten.Add(gorgonzola);
            zutaten.Add(rucola);
            zutaten.Add(tomaten);
            zutaten.Add(broccoli);
            zutaten.Add(artischocken);
            zutaten.Add(schokolade);
            zutaten.Add(scharfeSalami);
            zutaten.Add(knoblauch);
            zutaten.Add(thon);
            zutaten.Add(rohschinken);

            zutaten = zutaten.OrderBy(o => o.Name).ToList();

            foreach (Zutat zutat in zutaten)
            {
                coxExtrazutaten.Items.Add(zutat.Name + " " + zutat.Aufpreis + " CHF");
            }

            UpdateBestellungen();
        }

        private void UpdateBestellungen()
        {
            lstBestellungen.Items.Clear();

            if (!edited)
            {
                foreach (Bestellung bestellung in bestellungController.ReadBestellung())
                {
                    lstBestellungen.Items.Add($"Nr.{bestellung.Bestellnummer}: {bestellung.Kunde.Nachname} {bestellung.Kunde.Vorname}");
                }
            }
            else
            {
                Search();
                foreach(Bestellung bestellung in bestellungenFiltered)
                {
                    lstBestellungen.Items.Add($"Nr.{bestellung.Bestellnummer}: {bestellung.Kunde.Nachname} {bestellung.Kunde.Vorname}");
                }
            }
        }
        private void UpdateBestellpositionen()
        {
            lstBestellpositionen.Items.Clear();

            foreach (Bestellposition bestellposition in bestellpositionen)
            {
                //Hier könnte man noch die Extrazutaten anzeigen
                lstBestellpositionen.Items.Add($"{bestellposition.Pizza.Name} {bestellposition.Groesse.Name} {bestellposition.Stueckzahl}x -> {bestellposition.Preis} CHF");
            }
        }
        private void UpdateExtrazutaten()
        {
            lstZutaten.Items.Clear();

            foreach (Zutat zutat in extrazutatenAufPizza)
            {
                lstZutaten.Items.Add($"{zutat.Name} +{zutat.Aufpreis} CHF");
            }

            EinzelpreisBerechnen();
        }

        private void lstBestellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = lstBestellungen.SelectedIndex;

                if (index > -1)
                {
                    Bestellung bestellung = new Bestellung();
                    if (!edited)
                    {
                        bestellung = bestellungController.ReadBestellung()[index];
                    }
                    else
                    {
                        bestellung = bestellungenFiltered[index];
                    }

                    tbxBestellnummer.Text = bestellung.Bestellnummer.ToString();
                    tbxBestelldatum.Text = bestellung.Bestelldatum.ToString();
                    tbxTotal.Text = bestellung.Total.ToString();
                    int indexKunde = kunden.FindIndex(kunde => kunde._Id == bestellung.KundenId);
                    coxKunde.SelectedIndex = indexKunde;
                    coxAdresse.Items.Clear();
                    foreach (Adresse adresse in bestellung.Kunde.Adressen)
                    {
                        coxAdresse.Items.Add($"{adresse.Strasse} {adresse.Plz} {adresse.Ort}");
                    }
                    int indexAdresse = bestellung.Kunde.Adressen.FindIndex(adresse => adresse.Strasse == bestellung.LieferAdresse.Strasse && adresse.Plz == bestellung.LieferAdresse.Plz && adresse.Ort == bestellung.LieferAdresse.Ort);
                    coxAdresse.SelectedIndex = indexAdresse;
                    //Falls die Adresse vom Benutzer gelöscht wurde
                    if (coxAdresse.SelectedIndex == -1)
                    {
                        MessageBox.Show("WARNUNG: Ausgewählte Adresse wurde zuvor gelöscht. Bitte tragen Sie eine neue Adresse ein.");
                    }

                    bestellpositionen = new List<Bestellposition>(bestellung.Bestellpositionen);

                    UpdateBestellpositionen();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void lstBestellpositionen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = lstBestellpositionen.SelectedIndex;

                if (index > -1)
                {
                    Bestellposition bestellposition = bestellpositionen[index];

                    int indexPizza = pizzas.FindIndex(pizza => pizza._Id == bestellposition.PizzaId);
                    coxPizza.SelectedIndex = indexPizza;
                    coxGroesse.Items.Clear();
                    foreach (Groesse groesse in bestellposition.Pizza.Groessen)
                    {
                        coxGroesse.Items.Add($"{groesse.Name}: {groesse.GroesseInCm}cm +{groesse.Aufpreis} CHF");
                    }
                    int indexGroesse = bestellposition.Pizza.Groessen.FindIndex(groesse => groesse.Name == bestellposition.Groesse.Name);
                    coxGroesse.SelectedIndex = indexGroesse;
                    tbxStueckAnzahl.Text = bestellposition.Stueckzahl.ToString();
                    extrazutatenAufPizza = new List<Zutat>(bestellposition.Extrazutaten);
                    tbxPreis.Text = bestellposition.Preis.ToString();

                    UpdateExtrazutaten();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void coxPizza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = coxPizza.SelectedIndex;

            coxGroesse.Items.Clear();
            foreach (Groesse groesse in pizzas[index].Groessen)
            {
                coxGroesse.Items.Add($"{groesse.Name}: {groesse.GroesseInCm}cm +{groesse.Aufpreis} CHF");
            }

            EinzelpreisBerechnen();
        }

        private void btnAddZutat_Click(object sender, RoutedEventArgs e)
        {
            int index = coxExtrazutaten.SelectedIndex;
            if (index > -1)
            {
                Zutat zutat = zutaten[index];

                if (!extrazutatenAufPizza.Any(cus => cus.Name == zutat.Name))
                {
                    extrazutatenAufPizza.Add(zutat);
                    UpdateExtrazutaten();
                }
            }
        }

        private void btnZutatLoeschen_Click(object sender, RoutedEventArgs e)
        {
            int index = lstZutaten.SelectedIndex;
            if (index > -1)
            {
                extrazutatenAufPizza.RemoveAt(index);
                UpdateExtrazutaten();
            }
        }

        private void btnBestellpositionHinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bestellposition bestellposition = new Bestellposition();

                bestellposition.Extrazutaten = extrazutatenAufPizza;
                bestellposition.Pizza = pizzas[coxPizza.SelectedIndex];
                bestellposition.Groesse = pizzas[coxPizza.SelectedIndex].Groessen[coxGroesse.SelectedIndex];
                bestellposition.Stueckzahl = Convert.ToInt32(tbxStueckAnzahl.Text);
                bestellposition.Preis = Convert.ToDouble(tbxPreis.Text);
                bestellposition.PizzaId = bestellposition.Pizza._Id;

                bestellpositionen.Add(bestellposition);

                UpdateBestellpositionen();

                TotalBerechnen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nicht alle Angaben korrekt eingetragen!\n");
            }
        }

        private void btnBestellpositionLoeschen_Click(object sender, RoutedEventArgs e)
        {
            int index = lstBestellpositionen.SelectedIndex;

            if (index > -1)
            {
                bestellpositionen.RemoveAt(index);

                UpdateBestellpositionen();

                TotalBerechnen();
            }
        }

        private void btnBestellpositionUpdaten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = lstBestellpositionen.SelectedIndex;

                if (index > -1)
                {
                    Bestellposition bestellposition = new Bestellposition();

                    bestellposition.Extrazutaten = extrazutatenAufPizza;
                    bestellposition.Pizza = pizzas[coxPizza.SelectedIndex];
                    bestellposition.Groesse = pizzas[coxPizza.SelectedIndex].Groessen[coxGroesse.SelectedIndex];
                    bestellposition.Stueckzahl = Convert.ToInt32(tbxStueckAnzahl.Text);
                    bestellposition.Preis = Convert.ToDouble(tbxPreis.Text);
                    bestellposition.PizzaId = bestellposition.Pizza._Id;

                    bestellpositionen[index] = bestellposition;

                    UpdateBestellpositionen();

                    TotalBerechnen();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nicht alle Angaben korrekt eingetragen!\n");
            }
        }

        private void btnHinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bestellung bestellung = new Bestellung();

                bestellung.GenerateId();
                bestellung.Total = Convert.ToDouble(tbxTotal.Text);
                bestellung.Bestellnummer = Convert.ToInt32(tbxBestellnummer.Text);
                bestellung.Bestelldatum = DateTime.ParseExact(tbxBestelldatum.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                bestellung.Kunde = kunden[coxKunde.SelectedIndex];
                bestellung.KundenId = bestellung.Kunde._Id;
                bestellung.Bestellpositionen = bestellpositionen;
                bestellung.LieferAdresse = bestellung.Kunde.Adressen[coxAdresse.SelectedIndex];

                if (!IsAnyNullOrEmpty(bestellung))
                {
                    bestellungController.AddBestellung(bestellung);

                    UpdateBestellungen();
                }
                else
                {
                    MessageBox.Show("Nicht alle Angaben korrekt eingetragen!\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nicht alle Angaben korrekt eingetragen!\n");
            }
        }

        private void btnLoeschen_Click(object sender, RoutedEventArgs e)
        {
            int index = lstBestellungen.SelectedIndex;

            if (index > -1)
            {
                ObjectId id = new ObjectId();
                if (!edited)
                {
                    id = bestellungController.ReadBestellung()[index]._Id;
                }
                else
                {
                    id = bestellungenFiltered[index]._Id;
                }
                bestellungController.DeleteBestellung(id);

                UpdateBestellungen();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = lstBestellungen.SelectedIndex;

                if (index > -1)
                {
                    Bestellung bestellung = new Bestellung();

                    if (!edited)
                    {
                        bestellung._Id = bestellungController.ReadBestellung()[index]._Id;
                    }
                    else
                    {
                        bestellung._Id = bestellungenFiltered[index]._Id;
                    }
                    bestellung.Total = Convert.ToDouble(tbxTotal.Text);
                    bestellung.Bestellnummer = Convert.ToInt32(tbxBestellnummer.Text);
                    bestellung.Bestelldatum = DateTime.ParseExact(tbxBestelldatum.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    bestellung.Kunde = kunden[coxKunde.SelectedIndex];
                    bestellung.KundenId = bestellung.Kunde._Id;
                    bestellung.Bestellpositionen = bestellpositionen;
                    bestellung.LieferAdresse = bestellung.Kunde.Adressen[coxAdresse.SelectedIndex];

                    if (!IsAnyNullOrEmpty(bestellung))
                    {
                        bestellungController.UpdateBestellung(bestellung);

                        UpdateBestellungen();
                    }
                    else
                    {
                        MessageBox.Show("Nicht alle Angaben korrekt eingetragen!\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nicht alle Angaben korrekt eingetragen!\n");
            }
        }

        private bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            if (myObject.GetType() == typeof(Bestellung))
            {
                Bestellung bestellung = (Bestellung)myObject;

                if (bestellung.Bestellpositionen.Count <= 0)
                {
                    return true;
                }
                foreach (Bestellung b in bestellungController.ReadBestellung())
                {
                    if (b.Bestellnummer == bestellung.Bestellnummer && b._Id != bestellung._Id)
                    {
                        MessageBox.Show($"Bestellnummer ist nicht einzigartig!\nBestellnummer {bestellung.Bestellnummer} bereits gebraucht.");
                        return true;
                    }
                }
            }
            return false;
        }

        private void coxKunde_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = coxKunde.SelectedIndex;

            if (index > -1)
            {
                coxAdresse.Items.Clear();
                foreach (Adresse adresse in kunden[index].Adressen)
                {
                    coxAdresse.Items.Add($"{adresse.Strasse} {adresse.Plz} {adresse.Ort}");
                }
            }
        }

        private void TypeDouble(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !checkIfDouble(((TextBox)sender).Text + e.Text);
        }

        private bool checkIfDouble(string text)
        {
            bool succeeded = false;

            string regex = @"^\d*\.?\d*$";
            Match m = Regex.Match(text, regex);
            if (m.Success)
            {
                succeeded = true;
            }

            return succeeded;
        }
        private bool checkIfInt(string text)
        {
            bool succeeded = false;

            string regex = @"^\d+$";
            Match m = Regex.Match(text, regex);
            if (m.Success)
            {
                succeeded = true;
            }

            return succeeded;
        }
        private void TypeInt(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !checkIfInt(((TextBox)sender).Text + e.Text);
        }
        private void btnAddZutatToSelection_Click(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Name der Zutat:", "Temporäre Zutat hinzufügen", "");
            if (name != "")
            {
                double aufpreis = 5;

                try
                {
                    aufpreis = Convert.ToDouble(Microsoft.VisualBasic.Interaction.InputBox("Aufpreis der Zutat (Komma mit ',' - NICHT '.':", "Temporäre Zutat hinzufügen", ""));

                    if (aufpreis.ToString() == "")
                    {
                        aufpreis = 5;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Keine gültige Zahl! Neuer Aufpreis = 5");
                }


                Zutat zutat = new Zutat(name, aufpreis);
                zutaten.Add(zutat);
                zutaten = zutaten.OrderBy(o => o.Name).ToList();
                coxExtrazutaten.Items.Add(zutat.Name + " " + zutat.Aufpreis + " CHF");
                coxExtrazutaten.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
            }
        }

        //Bei jeder Eingabe, die den Preis verändert, wird der Einzelpreis automatisch berechnet und auf CHF gerundet (5 Rappen)
        private void EinzelpreisBerechnen()
        {
            double preis = 0;

            foreach (Zutat zutat in extrazutatenAufPizza)
            {
                preis += zutat.Aufpreis;
            }

            int indexGroesse = coxGroesse.SelectedIndex;
            int indexPizza = coxPizza.SelectedIndex;

            if (indexPizza > -1)
            {
                preis += pizzas[indexPizza].Einzelpreis;
            }

            if (indexGroesse > -1)
            {
                preis += pizzas[indexPizza].Groessen[indexGroesse].Aufpreis;
            }

            double multiplikator = 1;

            if (tbxStueckAnzahl.Text != "")
            {
                multiplikator = Convert.ToDouble(tbxStueckAnzahl.Text);
            }
            preis = preis * multiplikator;

            preis = Math.Round(preis * 20.0, MidpointRounding.AwayFromZero) * 0.05;
            preis = Math.Round(preis, 2);

            tbxPreis.Text = preis.ToString();
        }

        //Bei jeder Bestellposition wird das Total automatisch berechnet und auf CHF gerundet (5 Rappen)
        private void TotalBerechnen()
        {
            double total = 0;

            foreach (Bestellposition bestellposition in bestellpositionen)
            {
                total += bestellposition.Preis;
            }

            total = Math.Round(total * 20.0, MidpointRounding.AwayFromZero) * 0.05;
            total = Math.Round(total, 2);

            tbxTotal.Text = total.ToString();
        }

        private void coxGroesse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EinzelpreisBerechnen();
        }

        private void tbxStueckAnzahl_TextChanged(object sender, TextChangedEventArgs e)
        {
            EinzelpreisBerechnen();
        }

        private void FocusGotten(object sender, RoutedEventArgs e)
        {
            if (tbxBestelldatum.Text == "Z.B. 23.01:2023 12:52:43")
            {
                tbxBestelldatum.Text = "";
            }
        }

        private void FocusLost(object sender, RoutedEventArgs e)
        {
            if (tbxBestelldatum.Text == "")
            {
                tbxBestelldatum.Text = "Z.B. 23.01:2023 12:52:43";
            }
        }

        private void Search()
        {
            bestellungenFiltered.Clear();

            if (tbxSuche.Text != "")
            {
                foreach (Bestellung bestellung in bestellungController.ReadBestellung())
                {
                    if ($"Nr.{bestellung.Bestellnummer}: {bestellung.Kunde.Nachname} {bestellung.Kunde.Vorname}".Contains(tbxSuche.Text))
                    {
                        bestellungenFiltered.Add(bestellung);
                    }
                }

                edited = true;
            }
            else
            {
                edited = false;
            }
        }

        private void btnSuche_Click(object sender, RoutedEventArgs e)
        {
            Search();
            UpdateBestellungen();
        }
    }
}
