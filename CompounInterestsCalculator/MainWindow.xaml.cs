using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Controls;

namespace CompoundingCalculator
{
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();
            Labels = new string[0];
            YFormatter = value => value.ToString("C");

            DataContext = this;
            DrawGraph();
        }

        private void DrawGraph()
        {
            double initialAmount, monthlyDeposit, annualInterestRate, investmentPeriod;

            if (double.TryParse(initialAmountTextBox.Text, out initialAmount) &&
                double.TryParse(monthlyDepositTextBox.Text, out monthlyDeposit) &&
                double.TryParse(annualInterestRateTextBox.Text, out annualInterestRate) &&
                double.TryParse(investmentPeriodTextBox.Text, out investmentPeriod))
            {
                // Calculate compound interest
                double totalAmountWithCompounding = initialAmount;
                double totalAmountofInvestment = initialAmount;
                double anuallyInterestRate = annualInterestRate / 100;
                double monthlyInterestRate = annualInterestRate / 12 / 100;
                int numMonths = (int)(investmentPeriod * 12);
                int numYears = (int)(investmentPeriod);
                List<double> values_monthly_compound = new List<double>();
                List<double> values_anually_compound = new List<double>();
                List<double> values_investment_amount = new List<double>();
                List<double> values_rate_of_return = new List<double>();
                values_monthly_compound.Add(0);
                values_anually_compound.Add(0);
                values_investment_amount.Add(0);
                values_rate_of_return.Add(0);

                //연복리
                if (true)
                {
                    for (int i = 0; i < numYears; i++)
                    {
                        totalAmountWithCompounding += monthlyDeposit * 12;
                        totalAmountWithCompounding *= 1 + anuallyInterestRate;

                        totalAmountofInvestment += monthlyDeposit * 12;

                        values_anually_compound.Add(totalAmountWithCompounding);
                        values_investment_amount.Add(totalAmountofInvestment);
                        values_rate_of_return.Add(0);
                    }
                }
                else
                {
                    //월복리
                    for (int i = 0; i < numMonths; i++)
                    {
                        totalAmountWithCompounding += monthlyDeposit;
                        totalAmountofInvestment += monthlyDeposit;

                        totalAmountWithCompounding *= 1 + monthlyInterestRate;
                        if (i % 12 == 0)
                        {
                            values_monthly_compound.Add(totalAmountWithCompounding);
                            values_investment_amount.Add(totalAmountofInvestment);
                            values_rate_of_return.Add(0);
                        }
                    }
                }


                // Update chart
                SeriesCollection.Clear();
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "투자금",
                    Values = new ChartValues<double>(values_investment_amount),
                    MaxColumnWidth = 50,
                    //DataLabels = true,
                    LabelPoint = point => point.Y.ToString("C"),
                });
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "총액",
                    Values = new ChartValues<double>(values_anually_compound),
                    MaxColumnWidth = 50,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("C"),
                });
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "수익률",
                    Values = new ChartValues<double>(values_anually_compound),
                    MaxColumnWidth = 1,
                    //DataLabels = true,
                    LabelPoint = point => $"{(values_anually_compound[point.Key] / values_investment_amount[point.Key] * 100):N2}%",
                });
            }
            else
            {
                MessageBox.Show("입력값을 확인해주세요.");
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            DrawGraph();
        }
    }
}