using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sommerhåndbold
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HandballBiz biz = new HandballBiz();

        public MainWindow()
        {
            DataContext = biz;
            InitializeComponent();            
        }

        private void OpretPuljer_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> Seeds = new List<List<string>>();
            List<string> seed;

            seed = new List<string>();
            seed.Add(Seed1_1.SelectionBoxItem.ToString());
            seed.Add(Seed1_2.SelectionBoxItem.ToString());
            seed.Add(Seed1_3.SelectionBoxItem.ToString());
            seed.Add(Seed1_4.SelectionBoxItem.ToString());
            Seeds.Add(seed);

            seed = new List<string>();
            seed.Add(Seed2_1.SelectionBoxItem.ToString());
            seed.Add(Seed2_2.SelectionBoxItem.ToString());
            seed.Add(Seed2_3.SelectionBoxItem.ToString());
            seed.Add(Seed2_4.SelectionBoxItem.ToString());
            Seeds.Add(seed);

            seed = new List<string>();
            seed.Add(Seed3_1.SelectionBoxItem.ToString());
            seed.Add(Seed3_2.SelectionBoxItem.ToString());
            seed.Add(Seed3_3.SelectionBoxItem.ToString());
            seed.Add(Seed3_4.SelectionBoxItem.ToString());
            Seeds.Add(seed);

            seed = new List<string>();
            seed.Add(Seed4_1.SelectionBoxItem.ToString());
            seed.Add(Seed4_2.SelectionBoxItem.ToString());
            seed.Add(Seed4_3.SelectionBoxItem.ToString());
            seed.Add(Seed4_4.SelectionBoxItem.ToString());
            Seeds.Add(seed);

            biz.CreateGroups(Seeds);
        }

        private void AutoSeed_Click(object sender, RoutedEventArgs e)
        {
            Seed1_1.SelectedIndex = 0;
            Seed1_2.SelectedIndex = 1;
            Seed1_3.SelectedIndex = 2;
            Seed1_4.SelectedIndex = 3;
            Seed2_1.SelectedIndex = 4;
            Seed2_2.SelectedIndex = 5;
            Seed2_3.SelectedIndex = 6;
            Seed2_4.SelectedIndex = 7;
            Seed3_1.SelectedIndex = 8;
            Seed3_2.SelectedIndex = 9;
            Seed3_3.SelectedIndex = 10;
            Seed3_4.SelectedIndex = 11;
            Seed4_1.SelectedIndex = 12;
            Seed4_2.SelectedIndex = 13;
            Seed4_3.SelectedIndex = 14;
            Seed4_4.SelectedIndex = 15;
        }

        private void ButtonOpretKampe_Click(object sender, RoutedEventArgs e)
        {
            biz.OpretKampe();
            dataGridPuljeA.Items.Refresh();
        }

        private void RefreshGroups()
        {
            dataGridPuljeAMatches.Items.Refresh();
            dataGridPuljeA.Items.Refresh();
            dataGridPuljeBMatches.Items.Refresh();
            dataGridPuljeB.Items.Refresh();
            dataGridPuljeCMatches.Items.Refresh();
            dataGridPuljeC.Items.Refresh();
            dataGridPuljeDMatches.Items.Refresh();
            dataGridPuljeD.Items.Refresh();
        }

        private void RefreshIntermediates()
        {
            dataGridMellemrunde1.Items.Refresh();
            dataGridMellemrunde2.Items.Refresh();
            dataGridMellemrunde1Matches.Items.Refresh();
            dataGridMellemrunde2Matches.Items.Refresh();
        }

        private void ResultEnter(string home, string away)
        {
            try
            {
                int score1 = int.Parse(home);
                int score2 = int.Parse(away);
                biz.EnterResult(score1, score2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResultEnter_Click(object sender, RoutedEventArgs e)
        {
            ResultEnter(textBoxHjemmeScore.Text, textBoxUdeScore.Text);
            RefreshGroups();
        }


        private void DataGridPuljeAMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            biz.SelectedMatch = dataGridPuljeAMatches.SelectedItem as Match;
        }

        private void DataGridPuljeBMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            biz.SelectedMatch = dataGridPuljeBMatches.SelectedItem as Match;
        }

        private void DataGridPuljeCMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            biz.SelectedMatch = dataGridPuljeCMatches.SelectedItem as Match;
        }

        private void DataGridPuljeDMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            biz.SelectedMatch = dataGridPuljeDMatches.SelectedItem as Match;
        }

        private void ButtonSimulerResultater_Click(object sender, RoutedEventArgs e)
        {
            biz.SimulerResultater();
            RefreshGroups();
        }

        private void ButtonOpretMellemrunde_Click(object sender, RoutedEventArgs e)
        {
            biz.MakeIntermediate();
            RefreshIntermediates();
        }

        private void DataGridMellemrunde1Matches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            biz.SelectedMatch = dataGridMellemrunde1Matches.SelectedItem as Match;
            biz.SelectedGroup = biz.Intermediate1;
        }

        private void ButtonIntermediateResultEnter1_Click(object sender, RoutedEventArgs e)
        {
            ResultEnter(textBoxIntermediateScore1.Text, textBoxIntermediateScore2.Text);
            RefreshIntermediates();
        }

        private void DataGridMellemrunde2Matches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            biz.SelectedMatch = dataGridMellemrunde2Matches.SelectedItem as Match;
            biz.SelectedGroup = biz.Intermediate2;
        }

        private void ButtonIntermediateSimuler_Click(object sender, RoutedEventArgs e)
        {
            biz.SimulerResultater();
            RefreshIntermediates();
        }

        private void BtnFinalekampe_Click(object sender, RoutedEventArgs e)
        {
            biz.MakeFinals();
        }

        private void BtnSimulerFinale_Click(object sender, RoutedEventArgs e)
        {
            biz.SimulerResultater();
        }
    }
}
