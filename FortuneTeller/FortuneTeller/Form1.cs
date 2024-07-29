using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace FortuneTeller
{
    public partial class Form1 : Form
    {
        private const string appName = "Fortune Teller";
        private readonly string preditionsConfig = $"{Environment.CurrentDirectory}\\tsconfig1.json";
        private string[] predictions;
        private Random random = new Random();
        
        public Form1()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, EventArgs e)
        {
            button.Enabled = false;
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    this.Invoke(new Action(() =>
                    {
                        if(i == progressBar1.Maximum)
                        {
                            progressBar1.Maximum = i + 1;
                            progressBar1.Value = i + 1;
                            progressBar1.Maximum = i;
                        }
                        else
                        {
                            progressBar1.Value = i + 1;
                        }
                        progressBar1.Value = i;
                        this.Text = $"{i}%";
                    }));
                    Thread.Sleep(50);
                }
            });

            var index = random.Next(predictions.Length);
            var prediction = predictions[index];
            MessageBox.Show($"{prediction}");
            progressBar1.Value = 0;
            this.Text = appName;
            button.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = appName;
            try
            {
                var data = File.ReadAllText(preditionsConfig);
                predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(predictions == null)
                {
                    Close();
                }
                else if(predictions.Length == 0)
                {
                    MessageBox.Show("Предсказания закончились");
                    Close();
                }
            }
        }
    }
}
