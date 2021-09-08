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

namespace Klimatobservationer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservationDataBase db = new ObservationDataBase();
        Observer observer; 
        Observation observation; 
                
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods for UI
        /// <summary>
        /// Clears the texboxes
        /// </summary>
        public void ClearUI() 
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAddTemp.Clear();
            txtChangeValue.Clear();
        }
        /// <summary>
        /// Updates listbox on Click
        /// </summary>
        public void UpdateLstObservers() 
        {
            var observers = db.GetObservers();
            lstObservers.ItemsSource = null;
            lstObservers.ItemsSource = observers;
        }
        /// <summary>
        /// Updates listbox on Click
        /// </summary>
        public void UpdateLstMeasurement() 
        {
            var observation = (Observation)lstObservations.SelectedItem;
            var measurement = db.GetMeasurements(observation);
            lstEditMeasurements.ItemsSource = null;
            lstEditMeasurements.ItemsSource = measurement;            
        }
        /// <summary>
        /// Updates listbox on Click
        /// </summary>
        public void UpdateLstObservations() 
        {
            var observer = (Observer)lstObservers.SelectedItem;
            
            var observations = db.GetObservatons(observer);
            lstObservations.ItemsSource = null;
            lstObservations.ItemsSource = observations;
        }

        #endregion

        #region Buttons

        private void btnPresentObservations_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var observers = db.GetObservers();
                lstObservers.ItemsSource = null;
                lstObservers.ItemsSource = observers;
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        private void btnAddObserver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var observer = new Observer 
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text
                };

                db.AddObserver(observer);
                ClearUI();
                UpdateLstObservers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        private void btnRemoveObserver_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var observer = (Observer)lstObservers.SelectedItem; 
                var observations = db.GetObservatons(observer);  

                MessageBoxResult r = MessageBox.Show($"Vill du radera {lstObservers.SelectedItem} ur databasen?", "", MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                    db.RemoveObserver((Observer)lstObservers.SelectedItem);
                }                                
                ClearUI();                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

                MessageBoxResult r = MessageBox.Show($"Vill du flytta {lstObservers.SelectedItem}s observationer till en annan öbservatör?", "", MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                                        
                    MessageBox.Show($"Observationer raderade, {lstObservers.SelectedItem} kan nu raderaas från databasen");
                }

            }
            UpdateLstObservers();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var measurement = (Measurement)lstEditMeasurements.SelectedItem;
                int value = int.Parse(txtChangeValue.Text);
                db.UpdateMeasurement(measurement, value);
                txtChangeValue.Clear();
                UpdateLstMeasurement();
                MessageBox.Show("Mätningen är uppdaterad");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
            
        }

        private void btnAddMeasurement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = 0;
                if (radioFox.IsChecked == true)
                {
                    index = 10;
                }
                if (radioTemp.IsChecked == true)
                {
                    index = 8;
                }
                if (radioBirch.IsChecked == true)
                {
                    index = 9;
                }
                
                var observer = (Observer)lstObservers.SelectedItem;
                var observation = new Observation(); 
                var measurement = new Measurement();
                {
                    measurement.Value = int.Parse(txtAddTemp.Text);

                };

                db.AddObservation(observation, observer);
                db.AddMeasurementWithIndex(observation, measurement, index); 
                MessageBox.Show("Klimatobservation tillagd");
                UpdateLstObservations();
                ClearUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
            
            
        }

        #endregion

        #region DoubleClick
        private void lstObservers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
                        
            var observer = (Observer)lstObservers.SelectedItem;           
                       
            
            txtFirstName.Text = observer.FirstName;
            txtLastName.Text = observer.LastName;

            lblName.Content = $"{(Observer)lstObservers.SelectedItem}";           

            var observations = db.GetObservatons(observer);
            lstObservations.ItemsSource = null;
            lstObservations.ItemsSource = observations;
        }

        

        private void lstObservations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            var observation = (Observation)lstObservations.SelectedItem;           
                        
            var measurement = db.GetMeasurements(observation);
            lstEditMeasurements.ItemsSource = null;
            lstEditMeasurements.ItemsSource = measurement;
            
        }

        

        private void lstEditMeasurements_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {            
            var measurements = (Measurement)lstEditMeasurements.SelectedItem; 
            txtChangeValue.Text = measurements.Value.ToString();            
        }

        #endregion
    }
}
