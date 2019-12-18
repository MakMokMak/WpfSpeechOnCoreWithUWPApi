using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfSpeechOnCoreWithUWPApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Windows.Media.SpeechSynthesis.VoiceInformation[]> _voiceDic =
            new Dictionary<string, Windows.Media.SpeechSynthesis.VoiceInformation[]>();
        private readonly SelectGender _select;

        public MainWindow()
        {
            InitializeComponent();

            _select = new SelectGender();
            _select.SelectedGender = Windows.Media.SpeechSynthesis.VoiceGender.Male;
            this.RadioButtonGenderPanel.DataContext = _select;

            static void createDic(Dictionary<string, Windows.Media.SpeechSynthesis.VoiceInformation[]> voiceDic)
            {
                var allVoices = Windows.Media.SpeechSynthesis.SpeechSynthesizer.AllVoices;
                int genderNum;
                foreach (var voiceInfo in allVoices)
                {
                    if (!voiceDic.ContainsKey(voiceInfo.Language))
                    {
                        voiceDic.Add(voiceInfo.Language, new Windows.Media.SpeechSynthesis.VoiceInformation[2]);
                    }
                    genderNum = 0;
                    if (voiceInfo.Gender == Windows.Media.SpeechSynthesis.VoiceGender.Female)
                    {
                        genderNum = 1;
                    }
                    if (voiceDic[voiceInfo.Language][genderNum] == null)
                    {
                        voiceDic[voiceInfo.Language][genderNum] = voiceInfo;
                    }
                }
            }
            static void setComboboxItems(ComboBox comboBox, Dictionary<string, Windows.Media.SpeechSynthesis.VoiceInformation[]> voiceDic)
            {
                comboBox.ItemsSource = voiceDic;
                comboBox.DisplayMemberPath = "Key";
            }
            createDic(_voiceDic);
            setComboboxItems(this.ComboBoxLang, _voiceDic);
        }

        private async void ButtonDo_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.TextBoxInput.Text))
            {
                MessageBox.Show("発声するテキストを入力してください。");
                return;
            }
            var text = this.TextBoxInput.Text;
            if (this.ComboBoxLang.SelectedIndex < 0)
            {
                MessageBox.Show("発声する言語を選択してください。");
                return;
            }
            using var synthesizer = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            var i = 0;
            if (_select.SelectedGender == Windows.Media.SpeechSynthesis.VoiceGender.Female)
            {
                i = 1;
            }
            synthesizer.Voice = ((KeyValuePair<string, Windows.Media.SpeechSynthesis.VoiceInformation[]>)ComboBoxLang.SelectedItem).Value[i];
            using Windows.Media.SpeechSynthesis.SpeechSynthesisStream synthStream = await synthesizer.SynthesizeTextToStreamAsync(text);
            using Stream stream = synthStream.AsStreamForRead(); // WindowsRuntimeStreamExtensions.AsStreamForRead メソッド
            using var player = new System.Media.SoundPlayer();
            player.Stream = stream;
            player.Play();
        }
    }
}
