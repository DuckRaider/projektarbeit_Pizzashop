using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace projektarbeit_Pizzashop
{
    /// <summary>
    /// Interaktionslogik für PizzaView.xaml
    /// </summary>
    public partial class PizzaView : Window
    {
        private PizzaController pizzaController;
        //Potenzielle Zutaten, die zur Auswahl stehen
        private List<Zutat> zutaten;
        //Zutaten, die von der Auswahl auf die Pizza gepackt werden
        private List<Zutat> zutatenAufPizza;
        //Pizzas nach der Suche
        List<Pizza> pizzasFiltered;
        private bool edited;

        public PizzaView()
        {
            InitializeComponent();

            pizzaController = new PizzaController();
            zutaten = new List<Zutat>();
            zutatenAufPizza = new List<Zutat>();
            pizzasFiltered = new List<Pizza>();
            edited= false;

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
                coxZutaten.Items.Add(zutat.Name);
            }

            Update();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = lstPizzen.SelectedIndex;

                if (index > -1)
                {
                    Pizza pizza = new Pizza();

                    if(!edited)
                    {
                        pizza._Id = pizzaController.ReadPizza()[index]._Id;
                    }
                    else
                    {
                        pizza._Id = pizzasFiltered[index]._Id;
                    }
                    pizza.Name = tbxName.Text;
                    pizza.Einzelpreis = Convert.ToDouble(tbxEinzelpreis.Text);
                    //Ausgewählte Grössen eintragen
                    if ((bool)cbxSmall.IsChecked)
                    {
                        Groesse small = new Groesse("small", 24, Convert.ToDouble(tbxSmall.Text), Convert.ToDouble(tbxSmallKcal.Text));
                        pizza.AddGroesse(small);
                    }
                    if ((bool)cbxMedium.IsChecked)
                    {
                        Groesse medium = new Groesse("medium", 30, Convert.ToDouble(tbxMedium.Text), Convert.ToDouble(tbxMediumKcal.Text));
                        pizza.AddGroesse(medium);
                    }
                    if ((bool)cbxLarge.IsChecked)
                    {
                        Groesse large = new Groesse("large", 50, Convert.ToDouble(tbxLarge.Text), Convert.ToDouble(tbxLargeKcal.Text));
                        pizza.AddGroesse(large);
                    }
                    foreach (Zutat zutat in zutatenAufPizza)
                    {
                        pizza.AddZutat(zutat);
                    }

                    if (!IsAnyNullOrEmpty(pizza))
                    {
                        pizzaController.UpdatePizza(pizza);

                        Update();
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

        //Je nach Checkbox die Felder deaktivieren
        private void groesseChecked(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            bool cbxState = (bool)checkBox.IsChecked;

            switch (checkBox.Content.ToString())
            {
                case "Small":
                    if (cbxState)
                    {
                        tbxSmall.IsEnabled = true;
                        tbxSmallKcal.IsEnabled = true;
                    }
                    else
                    {
                        tbxSmall.IsEnabled = false;
                        tbxSmallKcal.IsEnabled = false;
                    }
                    break;
                case "Medium":
                    if (cbxState)
                    {
                        tbxMedium.IsEnabled = true;
                        tbxMediumKcal.IsEnabled = true;
                    }
                    else
                    {
                        tbxMedium.IsEnabled = false;
                        tbxMediumKcal.IsEnabled = false;
                    }
                    break;
                case "Large":
                    if (cbxState)
                    {
                        tbxLarge.IsEnabled = true;
                        tbxLargeKcal.IsEnabled = true;
                    }
                    else
                    {
                        tbxLarge.IsEnabled = false;
                        tbxLargeKcal.IsEnabled = false;
                    }
                    break;
                default:
                    MessageBox.Show("Error appeared");
                    break;
            }
        }

        //Auswahl Pizza Liste ändert sich
        private void lstPizzen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstPizzen.SelectedIndex;

            if (index > -1)
            {
                //Performance probleme -> lieber entfernen
                /*tbxSmall.Text = "";
                tbxMedium.Text = "";
                tbxLarge.Text = "";
                tbxSmallKcal.Text = "";
                tbxMediumKcal.Text = "";
                tbxLargeKcal.Text = "";*/
                cbxSmall.IsChecked = false;
                cbxMedium.IsChecked = false;
                cbxLarge.IsChecked = false;
                tbxSmall.IsEnabled = false;
                tbxMedium.IsEnabled = false;
                tbxLarge.IsEnabled = false;
                tbxSmallKcal.IsEnabled = false;
                tbxMediumKcal.IsEnabled = false;
                tbxLargeKcal.IsEnabled = false;

                Pizza pizza = new Pizza();
                if (!edited)
                {
                    pizza = pizzaController.ReadPizza()[index];
                }
                else
                {
                    pizza = pizzasFiltered[index];
                }

                zutatenAufPizza = new List<Zutat>(pizza.Zutaten);
                tbxName.Text = pizza.Name;

                foreach (Groesse groesse in pizza.Groessen)
                {
                    if (groesse.Name == "small")
                    {
                        cbxSmall.IsChecked = true;
                        tbxSmall.IsEnabled = true;
                        tbxSmallKcal.IsEnabled = true;
                        tbxSmall.Text = groesse.Aufpreis.ToString();
                        tbxSmallKcal.Text = groesse.Kcal.ToString();
                    }
                    if (groesse.Name == "medium")
                    {
                        cbxMedium.IsChecked = true;
                        tbxMedium.IsEnabled = true;
                        tbxMediumKcal.IsEnabled = true;
                        tbxMedium.Text = groesse.Aufpreis.ToString();
                        tbxMediumKcal.Text = groesse.Kcal.ToString();
                    }
                    if (groesse.Name == "large")
                    {
                        cbxLarge.IsChecked = true;
                        tbxLarge.IsEnabled = true;
                        tbxLargeKcal.IsEnabled = true;
                        tbxLarge.Text = groesse.Aufpreis.ToString();
                        tbxLargeKcal.Text = groesse.Kcal.ToString();
                    }
                }

                tbxEinzelpreis.Text = pizza.Einzelpreis.ToString();

                UpdateZutaten();
            }
        }

        //Zutat wird auf die Pizza gepackt
        private void btnAddZutat_Click(object sender, RoutedEventArgs e)
        {
            int index = coxZutaten.SelectedIndex;
            if (index > -1)
            {
                Zutat zutat = zutaten[index];

                if (!zutatenAufPizza.Any(cus => cus.Name == zutat.Name))
                {
                    zutatenAufPizza.Add(zutat);
                    UpdateZutaten();
                }
            }
        }

        //Alle Zutaten werden nochmals aufgelistet
        private void UpdateZutaten()
        {
            lstZutaten.Items.Clear();

            zutaten = zutaten.OrderBy(o => o.Name).ToList();

            foreach (Zutat zutat in zutatenAufPizza)
            {
                lstZutaten.Items.Add(zutat.Name);
            }
        }

        //Alle Pizzas werden nochmals aufgelistet
        private void Update()
        {
            lstPizzen.Items.Clear();

            if (!edited)
            {
                foreach (Pizza pizza in pizzaController.ReadPizza())
                {
                    lstPizzen.Items.Add(pizza.Name);
                }
            }
            else
            {
                Search();
                foreach (Pizza pizza in pizzasFiltered)
                {
                    lstPizzen.Items.Add(pizza.Name);
                }
            }
        }

        //Zutat von Pizza entfernen
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = lstZutaten.SelectedIndex;
            if (index > -1)
            {
                zutatenAufPizza.RemoveAt(index);
                UpdateZutaten();
            }
        }

        private void btnHinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Pizza pizza = new Pizza();

                pizza.GenerateId();
                pizza.Name = tbxName.Text;
                pizza.Einzelpreis = Convert.ToDouble(tbxEinzelpreis.Text);
                if ((bool)cbxSmall.IsChecked)
                {
                    Groesse small = new Groesse("small", 24, Convert.ToDouble(tbxSmall.Text), Convert.ToDouble(tbxSmallKcal.Text));
                    pizza.AddGroesse(small);
                }
                if ((bool)cbxMedium.IsChecked)
                {
                    Groesse medium = new Groesse("medium", 30, Convert.ToDouble(tbxMedium.Text), Convert.ToDouble(tbxMediumKcal.Text));
                    pizza.AddGroesse(medium);
                }
                if ((bool)cbxLarge.IsChecked)
                {
                    Groesse large = new Groesse("large", 50, Convert.ToDouble(tbxLarge.Text), Convert.ToDouble(tbxLargeKcal.Text));
                    pizza.AddGroesse(large);
                }
                foreach (Zutat zutat in zutatenAufPizza)
                {
                    pizza.AddZutat(zutat);
                }
                if (!IsAnyNullOrEmpty(pizza))
                {
                    pizzaController.AddPizza(pizza);

                    Update();
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
            int index = lstPizzen.SelectedIndex;

            if (index > -1)
            {
                ObjectId id = new ObjectId();
                if (!edited)
                {
                    id = pizzaController.ReadPizza()[index]._Id;
                }
                else
                {
                    id = pizzasFiltered[index]._Id;
                }
                pizzaController.DeletePizza(id);

                Update();
            }
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


        private void TypeNumericValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !checkIfDouble(((TextBox)sender).Text + e.Text);
        }


        //Überprüfen, ob alle Attribute in der Pizza Klasse gefüllt sind + weitere Validationen
        private bool IsAnyNullOrEmpty(Pizza myObject)
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
            //Überprüfen, ob Min. 1 Zutat und Grösse hinzugefügt wurde
            if (myObject.Zutaten.Count <= 0 || myObject.Groessen.Count <= 0)
            {
                return true;
            }
            return false;
        }

        //Eigene Zutat hinzufügen zur Selektion
        private void btnAddZutatToSelection_Click(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Name der Zutat:", "Temporäre Zutat hinzufügen", "");

            Zutat zutat = new Zutat(name, 0);
            zutaten.Add(zutat);
            zutaten = zutaten.OrderBy(o => o.Name).ToList();
            coxZutaten.Items.Add(zutat.Name);
            coxZutaten.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void Search()
        {
            pizzasFiltered.Clear();

            if(tbxSuche.Text != "")
            {
                foreach (Pizza pizza in pizzaController.ReadPizza())
                {
                    if (pizza.Name.Contains(tbxSuche.Text))
                    {
                        pizzasFiltered.Add(pizza);
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
            Update();
        }
    }
}
