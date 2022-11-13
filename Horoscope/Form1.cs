using Horoscope.DAL;
using Horoscope.DAL.Repositories;
using Horoscope.Models;
using Horoscope.Models.Enums;
using System.Text.RegularExpressions;
using ZodiakSign = Horoscope.Models.Enums.ZodiakSign;

namespace Horoscope
{
    public partial class Form1 : Form
    {
        private ZodiakSignRepository _zodiakSignRepository;
        private Dictionary<string, DivinationTime> _divinationTimes = new Dictionary<string, DivinationTime>()
        {
            { "сьогодні", DivinationTime.Today},
            { "завтра", DivinationTime.Tomorrow},
        };
        private Dictionary<ZodiakSign, string> _zodiakSignTranslation = new Dictionary<ZodiakSign, string>()
        {
            { ZodiakSign.Aries, "Овен" },
            { ZodiakSign.Taurus, "Телець" },
            { ZodiakSign.Gemini, "Близнюки" },
            { ZodiakSign.Cancer, "Рак" },
            { ZodiakSign.Leo, "Лев" },
            { ZodiakSign.Virgo, "Діва" },
            { ZodiakSign.Libra, "Терези" },
            { ZodiakSign.Scorpio, "Скорпіон" },
            { ZodiakSign.Sagittarius, "Стрілець" },
            { ZodiakSign.Capricorn, "Козеріг" },
            { ZodiakSign.Aquarius, "Водолій" },
            { ZodiakSign.Pisces, "Риби" },
        };

        public Form1()
        {
            InitializeComponent();

            HoroscopeContext horoscopeContext = new HoroscopeContext();
            _zodiakSignRepository = new ZodiakSignRepository(horoscopeContext);

            dateTimePicker1.MaxDate = DateTime.Now;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ValidateChildren(ValidationConstraints.Enabled);

            var person = new Person()
            {
                Name = textBox1.Text,
                Surname = textBox2.Text,
                Sex = radioButton1.Checked ? Sex.Male : Sex.Female,
                DateOfBirth = dateTimePicker1.Value
            };

            var divinationDate = _divinationTimes[comboBox2.Text] == DivinationTime.Today
                ? DateTime.UtcNow
                : DateTime.UtcNow.AddDays(1);

            string answer = CheckIfRecordExists(person, divinationDate)
                ? GetAnswerFromFile(person, divinationDate)
                : await GetAnswer(person, divinationDate);

            richTextBox1.Text = answer;
        }

        private bool CheckIfRecordExists(Person person, DateTime divinationDate)
        {
            string fullPath = $"../../../../results/{divinationDate.Day}-{divinationDate.Month}-{divinationDate.Year}.txt";

            if (!File.Exists(fullPath))
            {
                return false;
            }

            string lineToFind = $"{person.Name} | {person.Surname} | {person.Sex} | {person.DateOfBirth}";

            using (var reader = new StreamReader(fullPath))
            {
                string? line = reader.ReadLine();

                while (line != null)
                {
                    if (line.Contains(lineToFind))
                    {
                        return true;
                    }

                    line = reader.ReadLine();
                }
            }

            return false;
        }

        private string GetAnswerFromFile(Person person, DateTime divinationDate)
        {
            string fullPath = $"../../../../results/{divinationDate.Day}-{divinationDate.Month}-{divinationDate.Year}.txt";

            string lineToFind = $"{person.Name} | {person.Surname} | {person.Sex} | {person.DateOfBirth} | ";

            using (var reader = new StreamReader(fullPath))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    if (line.Contains(lineToFind))
                    {
                        return line.Replace(lineToFind, "");
                    }

                    line = reader.ReadLine();
                }
            }

            return string.Empty;
        }

        private async Task<string> GetAnswer(Person person, DateTime divinationDate)
        {
            var zodiakSign = GetZodiakSign(person.DateOfBirth.Month, person.DateOfBirth.Day);
            var answers = await GetAnswerData(zodiakSign);
            var answerIndex = new Random().Next(answers.Count);
            var answer = $"Ваш знак зодіаку: {_zodiakSignTranslation[zodiakSign]}. Передбачення для вас: {answers[answerIndex]}";

            WriteResultToFile(person, divinationDate, answer);

            return answer;
        }

        private ZodiakSign GetZodiakSign(int month, int day)
        {
            if (((month == 3) && (day >= 21 || day <= 31)) || ((month == 4) && (day >= 1 || day <= 20)))
            {
                return ZodiakSign.Aries;
            }
            if (((month == 4) && (day >= 21 || day <= 31)) || ((month == 5) && (day >= 1 || day <= 21)))
            {
                return ZodiakSign.Taurus;
            }
            if (((month == 5) && (day >= 21 || day <= 31)) || ((month == 6) && (day >= 1 || day <= 21)))
            {
                return ZodiakSign.Gemini;
            }
            if (((month == 6) && (day >= 22 || day <= 31)) || ((month == 7) && (day >= 1 || day <= 22)))
            {
                return ZodiakSign.Cancer;
            }
            if (((month == 7) && (day >= 23 || day <= 31)) || ((month == 8) && (day >= 1 || day <= 22)))
            {
                return ZodiakSign.Leo;
            }
            if (((month == 8) && (day >= 23 || day <= 31)) || ((month == 9) && (day >= 1 || day <= 21)))
            {
                return ZodiakSign.Virgo;
            }
            if (((month == 9) && (day >= 22 || day <= 31)) || ((month == 10) && (day >= 1 || day <= 22)))
            {
                return ZodiakSign.Libra;
            }
            if (((month == 10) && (day >= 23 || day <= 31)) || ((month == 11) && (day >= 1 || day <= 21)))
            {
                return ZodiakSign.Scorpio;
            }
            if (((month == 11) && (day >= 22 || day <= 31)) || ((month == 12) && (day >= 1 || day <= 21)))
            {
                return ZodiakSign.Sagittarius;
            }
            if (((month == 12) && (day >= 22 || day <= 31)) || ((month == 1) && (day >= 1 || day <= 20)))
            {
                return ZodiakSign.Capricorn;
            }
            if (((month == 1) && (day >= 21 || day <= 31)) || ((month == 2) && (day >= 1 || day <= 19)))
            {
                return ZodiakSign.Aquarius;
            }
            if (((month == 2) && (day >= 20 || day <= 31)) || ((month == 3) && (day >= 1 || day <= 20)))
            {
                return ZodiakSign.Pisces;
            }

            return ZodiakSign.Pisces;
        }

        private async Task<List<string>> GetAnswerData(ZodiakSign zodiakSign)
        {
            var zodiakSignEntity = await _zodiakSignRepository.GetByName(_zodiakSignTranslation[zodiakSign]);

            var answers = zodiakSignEntity.Predictions
                .Select(p => p.Text)
                .ToList();

            return answers;
        }

        private void WriteResultToFile(Person person, DateTime divinationDate, string answer)
        {
            string fullPath = $"../../../../results/{divinationDate.Day}-{divinationDate.Month}-{divinationDate.Year}.txt";

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Dispose();
            }

            using (var writer = new StreamWriter(fullPath, true))
            {
                writer.Write(person.Name);
                writer.Write(" | ");
                writer.Write(person.Surname);
                writer.Write(" | ");
                writer.Write(person.Sex);
                writer.Write(" | ");
                writer.Write(person.DateOfBirth);
                writer.Write(" | ");
                writer.WriteLine(answer);
            }
        }

        private void radioButton1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                e.Cancel = true;
                errorProvider1.SetError(radioButton1, "Необхідно обрати стать");
                errorProvider1.SetError(radioButton2, "Необхідно обрати стать");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(radioButton1, "");
                errorProvider1.SetError(radioButton2, "");
            }
        }

        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CheckIfNullOrEmpty(textBox1, e)) return;
            if (!CheckIfOnlyText(textBox1, e)) return;
        }

        private void textBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CheckIfNullOrEmpty(textBox2, e)) return;
            if (!CheckIfOnlyText(textBox2, e)) return;
        }

        private void comboBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CheckIfComboBoxIsUnchecked(comboBox2, e)) return;
        }

        private bool CheckIfNullOrEmpty(TextBox textBox, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "Необхідно заповнити поле");
                return true;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
                return false;
            }
        }

        private bool CheckIfOnlyText(TextBox textBox, System.ComponentModel.CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBox.Text, @"^[a-zA-ZА-Яа-яёЁЇїІіЄєҐґ']+$"))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "Необхідно ввести у поле тільки текст");
                return false;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
                return true;
            }
        }

        private bool CheckIfComboBoxIsUnchecked(ComboBox comboBox, System.ComponentModel.CancelEventArgs e)
        {
            if (comboBox.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(comboBox, "Необхідно обрати значення із випадаючого списку");
                return false;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(comboBox, "");
                return true;
            }
        }
    }
}