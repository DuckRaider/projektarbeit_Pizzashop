using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace projektarbeit_Pizzashop
{
    /// <summary>
    /// Interaktionslogik für KundeView.xaml
    /// </summary>
    public partial class KundeView : Window
    {
        List<Adresse> adressen;
        KundenController kundenController;
        //Suche
        List<Kunde> kundenFiltered;
        private bool edited;

        public KundeView()
        {
            InitializeComponent();
            adressen = new List<Adresse>();
            kundenController = new KundenController();
            kundenFiltered = new List<Kunde>();
            edited = false;

            Update();
        }

        private void btnHinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Kunde kunde = new Kunde();

                kunde.GenerateId();
                kunde.Vorname = tbxVorname.Text;
                kunde.Nachname = tbxNachname.Text;
                kunde.Email = tbxEmail.Text;
                kunde.Telefon = tbxTelefon.Text;
                //Nur das Datum nötig, Uhrzeit nicht
                kunde.Geburtsdatum = DateTime.ParseExact(tbxGeburtsdatum.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                kunde.Adressen = adressen;

                if (!IsAnyNullOrEmpty(kunde))
                {
                    kundenController.AddKunde(kunde);

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
        private bool IsAnyNullOrEmpty(Kunde myObject)
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
            //Kunde braucht min. 1 Adresse
            if (myObject.Adressen.Count <= 0)
            {
                return true;
            }
            //Regex für Email mit Sonderzeichen
            if (!Regex.IsMatch(myObject.Email, "^[\\.\\!\\#\\%\\&\\'\\*\\+\\-\\=\\?\\^\\`\\w\\/]+@[\\.\\!\\#\\%\\&\\'\\*\\+\\-\\=\\?\\^\\`\\w\\/]+\\.\\w+$"))
            {
                return true;
            }
            return false;
        }

        //Aktualisiere Adressenliste
        private void UpdateAdressen()
        {
            lstAdressen.Items.Clear();

            foreach (Adresse adresse in adressen)
            {
                lstAdressen.Items.Add($"{adresse.Strasse} {adresse.Plz} {adresse.Ort}");
            }
        }

        //Aktualisiere Kundenliste
        private void Update()
        {
            lstKunden.Items.Clear();

            if (!edited)
            {
                foreach (Kunde kunde in kundenController.ReadKunde())
                {
                    lstKunden.Items.Add($"{kunde.Nachname} {kunde.Vorname}");
                }
            }
            else
            {
                Search();
                foreach(Kunde kunde in kundenFiltered)
                {
                    lstKunden.Items.Add($"{kunde.Nachname} {kunde.Vorname}");
                }
            }
        }

        private void btnAddAdresse_Click(object sender, RoutedEventArgs e)
        {
            if (tbxStrasse.Text != "" && tbxPlz.Text != "" && tbxOrt.Text != "")
            {
                Adresse adresse = new Adresse();

                adresse.Strasse = tbxStrasse.Text;
                adresse.Ort = tbxOrt.Text;
                adresse.Plz = tbxPlz.Text;

                adressen.Add(adresse);

                UpdateAdressen();
            }
        }

        private void btnAdresseLoeschen_Click(object sender, RoutedEventArgs e)
        {
            int index = lstAdressen.SelectedIndex;
            if (index > -1)
            {
                adressen.RemoveAt(index);
                UpdateAdressen();
            }
        }

        private void lstKunden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstKunden.SelectedIndex;

            if (index > -1)
            {
                Kunde kunde = new Kunde();
                if (!edited)
                {
                    kunde = kundenController.ReadKunde()[index];
                }
                else
                {
                    kunde = kundenFiltered[index];
                }

                adressen = kunde.Adressen;
                tbxVorname.Text = kunde.Vorname;
                tbxNachname.Text = kunde.Nachname;
                tbxEmail.Text = kunde.Email;
                tbxTelefon.Text = kunde.Telefon;
                tbxGeburtsdatum.Text = kunde.Geburtsdatum.ToString("dd.MM.yyyy");
                adressen = new List<Adresse>(kunde.Adressen);

                UpdateAdressen();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = lstKunden.SelectedIndex;

                if (index > -1)
                {
                    Kunde kunde = new Kunde();

                    if (!edited)
                    {
                        kunde._Id = kundenController.ReadKunde()[index]._Id;
                    }
                    else
                    {
                        kunde._Id = kundenFiltered[index]._Id;
                    }
                    kunde.Vorname = tbxVorname.Text;
                    kunde.Nachname = tbxNachname.Text;
                    kunde.Email = tbxEmail.Text;
                    kunde.Telefon = tbxTelefon.Text;
                    kunde.Geburtsdatum = DateTime.ParseExact(tbxGeburtsdatum.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    kunde.Adressen = adressen;

                    if (!IsAnyNullOrEmpty(kunde))
                    {
                        kundenController.UpdateKunde(kunde);

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

        private void btnLoeschen_Click(object sender, RoutedEventArgs e)
        {
            int index = lstKunden.SelectedIndex;

            if (index > -1)
            {
                ObjectId id = new ObjectId();
                if (!edited)
                {
                    id = kundenController.ReadKunde()[index]._Id;
                }
                else
                {
                    id = kundenFiltered[index]._Id;
                }
                kundenController.DeleteKunde(id);

                Update();
            }
        }

        //Wenn das Datum selektiert wird und der Platzhalter, verschwindet der Platzhalter
        private void FocusGotten(object sender, RoutedEventArgs e)
        {
            if (tbxGeburtsdatum.Text == "Z.B. 23.01:2023")
            {
                tbxGeburtsdatum.Text = "";
            }
        }

        //Wenn das Feld nicht mehr selektiert ist und der Inhalt leer ist, wird der Platzhalter eingesetzt
        private void FocusLost(object sender, RoutedEventArgs e)
        {
            if (tbxGeburtsdatum.Text == "")
            {
                tbxGeburtsdatum.Text = "Z.B. 23.01:2023";
            }
        }

        private void Search()
        {
            kundenFiltered.Clear();

            if (tbxSuche.Text != "")
            {
                foreach (Kunde kunde in kundenController.ReadKunde())
                {
                    if ($"{kunde.Nachname} {kunde.Vorname}".Contains(tbxSuche.Text))
                    {
                        kundenFiltered.Add(kunde);
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
