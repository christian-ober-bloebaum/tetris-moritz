using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tetris
{

    public partial class StartWindow : Window
    {

        // Ändere das in StartWindow.xaml.cs:
        private SaveData saveData = new SaveData();

        public StartWindow()
        {
            InitializeComponent();
            LoadAccounts();

        }

        private void OpenGame(UserProfile account)
        {
            MainWindow mainWindow = new MainWindow(account);
            mainWindow.Show();
            this.Close();

        }
        private void LoadAccounts()
        {
            saveData = ProfileManager.LoadData();
            AccountComboBox.ItemsSource = saveData.Accounts;
            AccountComboBox.DisplayMemberPath = "Name";

            var sortedAccounts = saveData.Accounts.OrderByDescending(a => a.Highscore).ToList();
            ScoreboardList.ItemsSource = sortedAccounts;

            // Letzten Account automatisch auswählen, falls vorhanden
            if (saveData.Accounts.Count > 0)
            {
                UserProfile? lastAccount = saveData.Accounts.Find(a => a.Name == saveData.LastActiveAccountName);
                if (lastAccount != null)
                {
                    AccountComboBox.SelectedItem = lastAccount;
                }
                else
                {
                    AccountComboBox.SelectedIndex = 0;
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountComboBox.SelectedItem is UserProfile selectedAccount)
            {
                // Speichern, welcher Account aktiv ist
                saveData.LastActiveAccountName = selectedAccount.Name;
                ProfileManager.SaveDataToFile(saveData);

                // Hauptspiel-Fenster öffnen und Account mitgeben
                // Startfenster schließen
                OpenGame(selectedAccount);
            }
            else
            {
                MessageBox.Show("Bitte wähle einen Account aus oder erstelle einen neuen.");
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            string name = NewAccountTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;

            if (saveData.Accounts.Exists(a => a.Name == name))
            {
                MessageBox.Show("Dieser Name existiert bereits!");
                return;
            }

            var newAcc = new UserProfile { Name = name, Highscore = 0, IsGuest = false };
            saveData.Accounts.Add(newAcc);
            saveData.LastActiveAccountName = newAcc.Name;
            ProfileManager.SaveDataToFile(saveData);

            // Direkt auswählen und ins Spiel starten
            OpenGame(newAcc);
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            int guestCount = saveData.Accounts.FindAll(a => a.IsGuest).Count + 1;
            var guestAcc = new UserProfile { Name = $"Gast_{guestCount}", Highscore = 0, IsGuest = true };
            OpenGame(guestAcc);
        }



    }
}